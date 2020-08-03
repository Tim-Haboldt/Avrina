using System.Collections.Generic;
using UnityEngine;

public abstract class InputController : MonoBehaviour
{
    /// <summary>
    ///  Defines the controller type
    /// </summary>
    public abstract MappingType type { get; }
    /// <summary>
    ///  Left wall trigger. Stores the information about all close wall to the left of the player
    /// </summary>
    [HideInInspector] public PlayerCollider wallSlideColliderLeft { private set; get; }
    /// <summary>
    ///  Right wall trigger. Stores the information about all close wall to the right of the player
    /// </summary>
    [HideInInspector] public PlayerCollider wallSlideColliderRight { private set; get; }
    /// <summary>
    ///  Ground trigger. Stores the information about all ground colliders the player is staying on
    /// </summary>
    [HideInInspector] public PlayerCollider onGroundCollider { private set; get; }
    /// <summary>
    ///  Ground trigger. Stores the information about all ground colliders the player is staying on
    /// </summary>
    [HideInInspector] public CapsuleCollider2D playerCollider { private set; get; }
    /// <summary>
    ///  Stores the current movement input
    /// </summary>
    public float movementInput { get; protected set; }
    /// <summary>
    ///  Stores the current aim direction as vector
    /// </summary>
    public Vector2 aimDirecton { get; protected set; }
    /// <summary>
    ///  Is the jump input pressed
    /// </summary>
    public bool jumpInput { get; protected set; }
    /// <summary>
    ///  Is the duck input pressed
    /// </summary>
    public bool duckInput { get; protected set; }
    /// <summary>
    ///  Is the look up input pressed
    /// </summary>
    public bool lookUpInput { get; protected set; }
    /// <summary>
    ///  Is the cast input pressed
    /// </summary>
    public bool castInput { get; protected set; }
    /// <summary>
    ///  Is the cancel input pressed
    /// </summary>
    public bool cancelInput { get; protected set; }
    /// <summary>
    ///  Is the water element input pressed
    /// </summary>
    public bool waterElementInput { get; protected set; }
    /// <summary>
    ///  Is the fire element input pressed
    /// </summary>
    public bool fireElementInput { get; protected set; }
    /// <summary>
    ///  Is the earth element input pressed
    /// </summary>
    public bool earthElementInput { get; protected set; }
    /// <summary>
    ///  Is the air element input pressed
    /// </summary>
    public bool airElementInput { get; protected set; }
    /// <summary>
    ///  Stores the information if the player is touching the ground
    /// </summary>
    public bool onGround { get; protected set; }
    /// <summary>
    ///  Stores the information if a wall is on the left side of the player
    /// </summary>
    public bool hasWallLeft { get; protected set; }
    /// <summary>
    ///  Stores the information if a wall is on the right side of the player
    /// </summary>
    public bool hasWallRight { get; protected set; }
    /// <summary>
    ///  Stores the information what material the ground object has the player is staying on.
    ///  If the player is not on ground the variable is null
    /// </summary>
    public GroundMaterial groundMaterial { get; protected set; }
    /// <summary>
    ///  Stores the information what material the wall object has the player is wallsliding on.
    ///  If the player is not wallsliding the variable is null
    /// </summary>
    public WallMaterial wallMaterial { get; protected set; }


    /**
     * <summary>
     *  Will be called at the start of the game.
     *  Sets the masks of the colliders.
     * </summary>
     */
    public void Init(CapsuleCollider2D playerCollider, PlayerCollider wallLeft, PlayerCollider wallRight, PlayerCollider ground, LayerMask groundMask)
    {
        this.playerCollider = playerCollider;
        this.onGroundCollider = ground;
        this.wallSlideColliderLeft = wallLeft;
        this.wallSlideColliderRight = wallRight;

        this.onGroundCollider.mask = groundMask;
        this.wallSlideColliderLeft.mask = groundMask;
        this.wallSlideColliderRight.mask = groundMask;

        this.movementInput = 0;
        this.duckInput = false;
        this.lookUpInput = false;
        this.cancelInput = false;
        this.castInput = false;
        this.jumpInput = false;
        this.aimDirecton = Vector2.right;
        this.waterElementInput = false;
        this.fireElementInput = false;
        this.airElementInput = false;
        this.earthElementInput = false;
    }

    /// <summary>
    ///  Handles all inputs
    /// </summary>
    private void Update()
    {
        this.HandleKeyInputs();
        this.ColliderUpdate();
    }

    /// <summary>
    ///  Handels all key inputs
    /// </summary>
    public abstract void HandleKeyInputs();

    /// <summary>
    ///  Udpates all collider states
    /// </summary>
    private void ColliderUpdate()
    {
        // Update all colliding states
        this.onGround = this.onGroundCollider.isTriggered;
        this.hasWallLeft = this.wallSlideColliderLeft.isTriggered;
        this.hasWallRight = this.wallSlideColliderRight.isTriggered;

        // Updates the material of the ground
        this.SetGroundMaterial();
        this.SetWallMaterial();
    }

    /// <summary>
    ///  Sets the ground material to the one with the highest material
    /// </summary>
    private void SetGroundMaterial()
    {
        GroundMaterial groundMaterial = null;
        int priority = -1;

        foreach (Collider2D collider in this.onGroundCollider.colliders)
        {
            ObjectMaterial material = collider.gameObject.GetComponent<ObjectMaterial>();
            if (material.groundMaterial.priority > priority)
            {
                groundMaterial = material.groundMaterial;
            }
        }

        this.groundMaterial = groundMaterial;
    }

    /// <summary>
    ///  Sets the wall material to the one with the highest material
    /// </summary>
    private void SetWallMaterial()
    {
        WallMaterial wallMaterial = null;
        int priority = -1;

        List<Collider2D> colliders = null;
        if (this.hasWallLeft)
        {
            colliders = this.wallSlideColliderLeft.colliders;
        }
        else
        {
            colliders = this.wallSlideColliderRight.colliders;
        }

        foreach (Collider2D collider in colliders)
        {
            ObjectMaterial material = collider.gameObject.GetComponent<ObjectMaterial>();
            if (material.wallMaterial.priority > priority)
            {
                wallMaterial = material.wallMaterial;
            }
        }

        this.wallMaterial = wallMaterial;
    }
}
