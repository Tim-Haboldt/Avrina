using UnityEngine;

public class MouseInputController : InputController
{
    /// <summary>
    ///  Defines the controller type
    /// </summary>
    public override MappingType type { get { return MappingType.Mouse; } }
    /// <summary>
    ///  Stores all keymappings of the player controller
    /// </summary>
    [SerializeField] private MouseMapping keyMapping = null;

    /// <summary>
    ///  Stores the position of the player. Used to get the direction of the mouse relative to the player
    /// </summary>
    private Transform playerTransform = null;


    /// <summary>
    ///  Used to extract the player transform from the collider
    /// </summary>
    /// <param name="playerCollider"></param>
    /// <param name="wallLeft"></param>
    /// <param name="wallRight"></param>
    /// <param name="ground"></param>
    /// <param name="groundMask"></param>
    public override void Init(CapsuleCollider2D playerCollider, PlayerCollider wallLeft, PlayerCollider wallRight, PlayerCollider ground, LayerMask groundMask)
    {
        base.Init(playerCollider, wallLeft, wallRight, ground, groundMask);

        this.playerTransform = playerCollider.transform;
    }

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
        var cameraPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var aimDirection = cameraPos - this.playerTransform.position;
        var nextAimInput = new Vector2(aimDirection.x, aimDirection.y).normalized;
        this.aimDirecton = nextAimInput == Vector2.zero ? this.aimDirecton : nextAimInput;
    }
}
