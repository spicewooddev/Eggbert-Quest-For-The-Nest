using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eggbert : MonoBehaviour
{
    private float speed = 10;
    private Rigidbody2D body;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);
  
        if (Input.GetKey("space"))
        {
            body.velocity = new Vector2(body.velocity.x, speed);
        }
    }
}
