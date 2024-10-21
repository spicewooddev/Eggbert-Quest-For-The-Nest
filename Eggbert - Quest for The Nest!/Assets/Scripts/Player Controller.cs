using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D body;
    private BoxCollider2D bodyCollider;

    [SerializeField] private LayerMask groundLayer;

    [Header("Movement Parameters")]
    public float moveSpeed;

    [Header("Jump Physics Parameters")]
    public float jumpHeight;
    public float gravityScale;
    public float fallGravityScale;

    bool isJumping;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        bodyCollider = GetComponent<BoxCollider2D>();

        isJumping = false;
    }

    //Player inputs go here
    private void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            isJumping = true;
        }
    }

    //For physics, we want to use FixedUpdate() instead of Update().
    void FixedUpdate()
    {
        //By using GetAxisRaw, the player will hard stop when they release the movement keys, providing them more control
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        body.velocity = new Vector2(horizontalInput * moveSpeed, body.velocity.y);

        if (isJumping && IsGrounded())
        {
            PlayerJump();
        }

        if (body.velocity.y > 0)
        {
            body.gravityScale = gravityScale;
        }
        else
        {
            body.gravityScale = fallGravityScale;
        }
    }

    void PlayerJump()
    {
        body.gravityScale = gravityScale;
        float jumpForce = Mathf.Sqrt(jumpHeight * (Physics2D.gravity.y * body.gravityScale) * -2) * body.mass;
        body.velocity = new Vector2(body.velocity.x, jumpForce);

        isJumping = false;
    }

    private bool IsGrounded()
    {
        //the vector added to the bodyCollider.bounds.size makes Coyote time possible
        RaycastHit2D feetRaycast = Physics2D.BoxCast(bodyCollider.bounds.center, bodyCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return feetRaycast.collider != null;
    }
}
