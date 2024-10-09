using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D body;

    [SerializeField] float moveSpeed = 10;

    private float jumpHeight = 5;
    [SerializeField] float gravityScale = 15;
    [SerializeField] float fallGravityScale = 20;

    bool isGrounded;
    bool isJumping;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();

        isGrounded = false;
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

        if (isJumping && isGrounded)
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

        //Commenting isGrounded out because this can cause a problem
        //if the player were to jump and an obstacle were above them

        //isGrounded = false;
        isJumping = false;
    }

    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
        
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }
}
