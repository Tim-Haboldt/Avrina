using UnityEngine;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    /// <summary>
    ///  Left wall trigger. Stores the information about all close wall to the left of the player
    /// </summary>
    [SerializeField] private PlayerCollider wallSlideColliderLeft;
    /// <summary>
    ///  Right wall trigger. Stores the information about all close wall to the right of the player
    /// </summary>
    [SerializeField] private PlayerCollider wallSlideColliderRight;
    /// <summary>
    ///  Ground trigger. Stores the information about all ground colliders the player is staying on
    /// </summary>
    [SerializeField] private PlayerCollider onGroundCollider;
    /// <summary>
    ///  What is the mask of the ground or wall objects
    /// </summary>
    [SerializeField] private LayerMask groundMask;
    /// <summary>
    ///  Stores all keymappings of the player controller
    /// </summary>
    [SerializeField] private KeyMappings keyMappings;
    /// <summary>
    ///  Stores the current movement input
    /// </summary>
    public float movementInput { get; private set; }
    /// <summary>
    ///  Is the jump input pressed
    /// </summary>
    public bool jumpInput { get; private set; }
    /// <summary>
    ///  Is the duck input pressed
    /// </summary>
    public bool duckInput { get; private set; }
    /// <summary>
    ///  Is the look up input pressed
    /// </summary>
    public bool lookUpInput { get; private set; }
    /// <summary>
    ///  Is the cast input pressed
    /// </summary>
    public bool castInput { get; private set; }
    /// <summary>
    ///  Is the cancel input pressed
    /// </summary>
    public bool cancelInput { get; private set; }
    /// <summary>
    ///  Is the water element input pressed
    /// </summary>
    public bool waterElementInput { get; private set; }
    /// <summary>
    ///  Is the fire element input pressed
    /// </summary>
    public bool fireElementInput { get; private set; }
    /// <summary>
    ///  Is the earth element input pressed
    /// </summary>
    public bool earthElementInput { get; private set; }
    /// <summary>
    ///  Is the air element input pressed
    /// </summary>
    public bool airElementInput { get; private set; }
    /// <summary>
    ///  Stores the information if the player is touching the ground
    /// </summary>
    public bool onGround { get; private set; }
    /// <summary>
    ///  Stores the information if a wall is on the left side of the player
    /// </summary>
    public bool hasWallLeft { get; private set; }
    /// <summary>
    ///  Stores the information if a wall is on the right side of the player
    /// </summary>
    public bool hasWallRight { get; private set; }
    /// <summary>
    ///  Stores the information what material the ground object has the player is staying on.
    ///  If the player is not on ground the variable is null
    /// </summary>
    public GroundMaterial groundMaterial { get; private set; }
    /// <summary>
    ///  Stores the information what material the wall object has the player is wallsliding on.
    ///  If the player is not wallsliding the variable is null
    /// </summary>
    public WallMaterial wallMaterial { get; private set; }


    /**
     * <summary>
     *  Will be called at the start of the game.
     *  Sets the masks of the colliders.
     * </summary>
     */ 
    private void Start()
    {
        this.onGroundCollider.mask = this.groundMask;
        this.wallSlideColliderLeft.mask = this.groundMask;
        this.wallSlideColliderRight.mask = this.groundMask;
    }

    /**
     * <summary>
     *  Will check for all inputs and updates them.
     * </summary>
     */ 
    void Update()
    {
        // Convert vertical movement axis to duck and look up input
        var verticalMovementInput = Input.GetAxisRaw(this.keyMappings.verticalMovement);
        this.duckInput = false;
        this.lookUpInput = false;

        if (verticalMovementInput > 0)
        {
            this.duckInput = true;
        }
        else if (verticalMovementInput < 0)
        {
            this.lookUpInput = true;
        }

        // Get the player Inputs and write them into the global variables
        this.movementInput = Input.GetAxisRaw(this.keyMappings.horizontalMovement);
        this.jumpInput = Input.GetKey(this.keyMappings.jump);
        this.castInput = Input.GetKey(this.keyMappings.cast);
        this.cancelInput = Input.GetKey(this.keyMappings.cancel);
        this.waterElementInput = Input.GetKey(this.keyMappings.waterElement);
        this.fireElementInput = Input.GetKey(this.keyMappings.fireElement);
        this.earthElementInput = Input.GetKey(this.keyMappings.earthElement);
        this.airElementInput = Input.GetKey(this.keyMappings.airElement);

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
        } else
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
