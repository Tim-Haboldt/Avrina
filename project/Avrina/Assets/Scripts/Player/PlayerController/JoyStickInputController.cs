using UnityEngine;

public class JoyStickInputController : InputController
{
    /// <summary>
    ///  Stores all keymappings of the player controller
    /// </summary>
    [SerializeField] private JoyStickMapping keyMapping;
    /// <summary>
    ///  Because element inputs should only last for one frame the input needs to be saved
    /// </summary>
    private bool earthInputWasPressedLastUpdate = false;
    /// <summary>
    ///  Because element inputs should only last for one frame the input needs to be saved
    /// </summary>
    private bool airInputWasPressedLastUpdate = false;


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
        this.duckInput = false;
        this.lookUpInput = false;
        var verticalMovement = Input.GetAxisRaw(this.keyMapping.verticalMovement);
        if (verticalMovement > 0)
        {

            this.lookUpInput = true;
        }
        else if (verticalMovement < 0)
        {

            this.duckInput = true;
        }
        // Reads and stores all magic system related inputs
        this.castInput = Input.GetKey(this.keyMapping.cast);
        this.cancelInput = Input.GetKey(this.keyMapping.cancel);
        this.waterElementInput = Input.GetKeyDown(this.keyMapping.waterElement);
        this.fireElementInput = Input.GetKeyDown(this.keyMapping.fireElement);

        var earthInput = Input.GetAxisRaw(this.keyMapping.earthElement) > 0.1;
        this.earthElementInput = (earthInput && !this.earthInputWasPressedLastUpdate);
        this.earthInputWasPressedLastUpdate = earthInput;

        var airInput = Input.GetAxisRaw(this.keyMapping.airElement) > 0.1;
        this.airElementInput = airInput && !this.airInputWasPressedLastUpdate;
        this.airInputWasPressedLastUpdate = airInput;
    }
}
