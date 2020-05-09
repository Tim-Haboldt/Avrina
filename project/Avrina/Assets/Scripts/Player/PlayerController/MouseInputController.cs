using UnityEngine;

public class MouseInputController : InputController
{
    /// <summary>
    ///  Stores all keymappings of the player controller
    /// </summary>
    [SerializeField] private MouseMapping keyMapping;


    /// <summary>
    ///  Checks for all inputs and sets the public variables corresponding
    /// </summary>
    private void Update()
    {
        // Updates all collider states
        this.ColliderUpdate();

        // Reads and stores all movement related inputs
        this.movementInput = Input.GetAxisRaw(this.keyMapping.horizontalMovement);
        this.jumpInput = Input.GetKey(this.keyMapping.jump);
        this.duckInput = Input.GetKey(this.keyMapping.duck);
        this.lookUpInput = Input.GetKey(this.keyMapping.lookUp);
        // Reads and stores all magic system related inputs
        this.castInput = Input.GetKey(this.keyMapping.cast);
        this.cancelInput = Input.GetKey(this.keyMapping.cancel);
        this.waterElementInput = Input.GetKeyDown(this.keyMapping.waterElement);
        this.fireElementInput = Input.GetKeyDown(this.keyMapping.fireElement);
        this.earthElementInput = Input.GetKeyDown(this.keyMapping.earthElement);
        this.airElementInput = Input.GetKeyDown(this.keyMapping.airElement);
    }
}
