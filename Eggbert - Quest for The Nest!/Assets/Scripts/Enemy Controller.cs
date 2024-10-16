using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public State currentState = State.Idle;

    GameObject player;
    [SerializeField] float movementSpeed = 3;

    float sightRange = 10f;

    float distanceToPlayer;
    Vector3 startingPoint;

    //we won't worry about the direction or angle just yet.
    Vector2 direction;
    float angle;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        startingPoint = transform.position;
    }

    void Update()
    {
        distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        /*
        direction = player.transform.position - transform.position;
        direction.Normalize();
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        */

        switch (currentState)
        {
            case State.Idle:
                //Default path
                //Ant is meant to walk back and forth in a designated path
                if (distanceToPlayer <= sightRange)
                {
                    currentState = State.Attack;
                }
                break;

            case State.Attack:
                //When the player is within the Ant's range of sight, the ant will follow the player
                //The ant will try to touch the player in an attempt to damage the player
                if (distanceToPlayer > sightRange)
                {
                    currentState = State.Return;
                }
                break;

            case State.Return:
                //Ant will return to where they were before detecting the player
                if (transform.position == startingPoint)
                {
                    currentState = State.Idle;
                }
                break;
        }
    }

    private void FixedUpdate()
    {
        if (currentState == State.Attack)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, movementSpeed * Time.deltaTime);
            //transform.rotation = Quaternion.Euler(Vector3.forward * angle);
        }

        else if (currentState == State.Return)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, startingPoint, movementSpeed * Time.deltaTime);
        }
    }
}

[System.Serializable]
public enum State { Idle, Attack, Return }