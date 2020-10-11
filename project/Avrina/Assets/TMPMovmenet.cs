using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class TMPMovmenet : MonoBehaviour
{
    [SerializeField] private KeyCode left;
    [SerializeField] private KeyCode right;
    [SerializeField] private KeyCode up;
    [SerializeField] private KeyCode down;

    [SerializeField] private float speed;

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        this.rb = GetComponent<Rigidbody2D>();
        this.rb.gravityScale = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var currentPos = this.rb.position;

        if (Input.GetKey(this.left))
        {
            currentPos.x -= speed;
        }
        else if (Input.GetKey(this.right))
        {
            currentPos.x += speed;
        }

        if (Input.GetKey(this.up))
        {
            currentPos.y += speed;
        }
        else if (Input.GetKey(this.down))
        {
            currentPos.y -= speed;
        }

        this.rb.position = currentPos;
    }
}
