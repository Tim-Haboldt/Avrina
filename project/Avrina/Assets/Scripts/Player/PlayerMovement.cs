using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerMovement : MonoBehaviour
{
    public float force;

    private PlayerInput inputs;
    private Rigidbody2D rb;
    
    void Start()
    {
        this.inputs = GetComponent<PlayerInput>();
        this.rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        this.rb.AddForce(new Vector2(this.force * this.inputs.movementInput, 0));
    }
}
