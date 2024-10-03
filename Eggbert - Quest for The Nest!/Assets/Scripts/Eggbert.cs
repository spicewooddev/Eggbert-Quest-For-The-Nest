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

    bool isGrounded = false;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    //For physics, we want to use FixedUpdate() instead of Update().
    void FixedUpdate()
    {
        //By using GetAxisRaw, the player will hard stop when they release the movement keys, providing them more control
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        body.velocity = new Vector2(horizontalInput * moveSpeed, body.velocity.y);
  
        if (Input.GetKey("space") && isGrounded)
        {
            body.gravityScale = gravityScale;
            float jumpForce = Mathf.Sqrt(jumpHeight * (Physics2D.gravity.y * body.gravityScale) * -2) * body.mass;
            body.velocity = new Vector2(body.velocity.x, jumpForce);
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isGrounded = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }
}
