using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerMovement : MonoBehaviour
{
    public float speed = 4f;

    private PlayerInput inputs;
    private Rigidbody2D rb;
    
    void Start()
    {
        this.inputs = GetComponent<PlayerInput>();
        this.rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        this.rb.MovePosition(this.rb.position + this.inputs.movementInput * this.speed * Time.deltaTime);
    }
}
