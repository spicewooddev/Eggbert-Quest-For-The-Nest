using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//from this video: https://www.youtube.com/watch?v=uOobLo2y3KI

public class EnemyDetector : MonoBehaviour
{
    [field: SerializeField]
    public bool PlayerDetected { get; private set; }

    public Vector2 DirectionToTarget => target.transform.position - detectorOrigin.position;

    [Header("OverlapBox Parameters")]
    [SerializeField] private Transform detectorOrigin;
    public Vector2 detectorSize = Vector2.one;
    public Vector2 detectorOriginOffset = Vector2.zero;

    public float detectionDelay = 0.3f;

    public LayerMask detectorLayerMask;

    [Header("Gizmo Parameters")]
    public Color gizmoIdleColor = Color.green;
    public Color gizmoDetectedColor = Color.red;
    public bool showGizmos = true;

    private GameObject target;

    public GameObject Target
    {
        get => target;
        private set
        {
            target = value;
            PlayerDetected = target != null;
        }
    }

    private void Start()
    {
        StartCoroutine(DetectionCoroutine());
    }

    IEnumerator DetectionCoroutine()
    {
        yield return new WaitForSeconds(detectionDelay);
        PerformCoroutine();
        StartCoroutine(DetectionCoroutine());
    }

    public void PerformCoroutine()
    {
        Collider2D collider = Physics2D.OverlapBox(
            (Vector2) detectorOrigin.position + detectorOriginOffset,
            detectorSize, 0, detectorLayerMask);

        if (collider != null)
        {
            Target = collider.gameObject;
        }
        else
        {
            Target = null;
        }
    }

    private void OnDrawGizmos()
    {
        if (showGizmos && detectorOrigin != null)
        {
            Gizmos.color = gizmoIdleColor;

            if (PlayerDetected)
            {
                Gizmos.color = gizmoDetectedColor;
            }

            Gizmos.DrawCube(
                    (Vector2) detectorOrigin.position + detectorOriginOffset,
                    detectorSize);
        }
    }
}
