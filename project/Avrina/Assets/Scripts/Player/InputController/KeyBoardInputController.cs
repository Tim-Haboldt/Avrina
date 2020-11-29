using UnityEngine;

public class KeyBoardInputController : InputController
{
    /// <summary>
    ///  Defines the controller type
    /// </summary>
    public override MappingType type { get { return MappingType.KeyBoard; } }
    /// <summary>
    ///  Stores all keymappings of the player controller
    /// </summary>
    [SerializeField] private KeyBoardMapping keyMapping;


    /// <summary>
    ///  Checks for all inputs and sets the public variables corresponding
    /// </summary>
    public override void HandleKeyInputs()
    {
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
        // Spell direction input
        var nextAimInput = (new Vector2(Input.GetAxisRaw(this.keyMapping.horizontalAim), Input.GetAxisRaw(this.keyMapping.verticalAim))).normalized;
        this.aimDirecton = nextAimInput == Vector2.zero ? this.aimDirecton : nextAimInput;
        // Reads and stores the setting input
        this.settingsInput = Input.GetKey(this.keyMapping.settings);
    }
}
