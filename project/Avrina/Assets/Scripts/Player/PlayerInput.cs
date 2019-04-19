using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float movementInput { get; private set; }
    public bool jumpInput { get; private set; }
    
    void Update()
    {
        // Get the player Inputs and write them into the global variables
        this.movementInput = Input.GetAxisRaw("Horizontal");
        this.jumpInput = Input.GetKey(KeyCode.Space);
    }
}
