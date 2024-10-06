using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Eggbert : MonoBehaviour
{
    private Rigidbody2D body;

    [SerializeField] float moveSpeed = 10;

    private float jumpHeight = 5;
    [SerializeField] float gravityScale = 15;
    [SerializeField] float fallGravityScale = 20;

    bool isGrounded;

    //int jumpCount;
    //public int maxJumps = 1; //maximum amount of jumps the player can perform

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();

        isGrounded = false;
        //jumpCount = maxJumps;
    }

    //For physics, we want to use FixedUpdate() instead of Update().
    void FixedUpdate()
    {
        //By using GetAxisRaw, the player will hard stop when they release the movement keys, providing them more control
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        body.velocity = new Vector2(horizontalInput * moveSpeed, body.velocity.y);

        PlayerJump();
    }

    void PlayerJump()
    {
        if (Input.GetKey("space") && isGrounded)
        {
            body.gravityScale = gravityScale;
            float jumpForce = Mathf.Sqrt(jumpHeight * (Physics2D.gravity.y * body.gravityScale) * -2) * body.mass;
            body.velocity = new Vector2(body.velocity.x, jumpForce);

            /*
            if (jumpCount > 0)
            {
                jumpCount -= 1;
            }
            */

            //isGrounded = false;
        }

        
        if (body.velocity.y > 0)
        {
            body.gravityScale = gravityScale;
            isGrounded = false;
        }
        else
        {
            body.gravityScale = fallGravityScale;
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
            //jumpCount = maxJumps;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
            isGrounded = false;
    }
}
