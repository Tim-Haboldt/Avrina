using UnityEngine;

public class WallJumping : StateInheritingAction
{
    /// <summary>
    ///  How long does the walljump can last
    /// </summary>
    private float wallJumpMaxDuration;
    /// <summary>
    ///  Lowers the gravity during the jump
    /// </summary>
    private float gravityDuringJump;
    /// <summary>
    ///  Start time of the air jump
    /// </summary>
    private float startTime;
    /// <summary>
    ///  The name of the state is Walljumping
    /// </summary>
    public override PlayerState name { get; } = PlayerState.WallJumping;
    /// <summary>
    ///  Defines all actions that can occour in the walljumping state.
    /// </summary>
    protected override Action[] actions { get; } = new Action[] 
    {
        new HorizontalAirMovement(),
    };
    /// <summary>
    ///  Sound which will be played if the player jumps
    /// </summary>
    private AudioClip jumpSound;


    /// <summary>
    ///  Changes to the state immobile if the jump is finished
    /// </summary>
    /// <returns></returns>
    public override PlayerState Update()
    {
        var passedTime = Time.time - this.startTime;

        if ((!this.inputController.jumpInput) || passedTime >= this.wallJumpMaxDuration)
        {
            return PlayerState.InAir;
        }

        return this.name;
    }

    /// <summary>
    ///  Sets the movement speed and direction of the player
    /// </summary>
    /// <param name="velocity"></param>
    protected override void PerformAction(ref Vector2 velocity)
    {
        velocity = new Vector2(velocity.x, velocity.y - this.gravityDuringJump);
    }

    /// <summary>
    ///  Reads some values from the config
    /// </summary>
    /// <param name="config"></param>
    protected override void Setup(PlayerConfig config)
    {
        this.wallJumpMaxDuration = config.maxJumpDuration;
        this.gravityDuringJump = config.gravityDuringJump;
        this.jumpSound = config.jumpSound;
    }

    /// <summary>
    ///  Sets the start time of the jump
    /// </summary>
    protected override void OnEnter()
    {
        this.startTime = Time.time;

        var wallMaterial = this.inputController.wallMaterial;
        var direction = Mathf.Sign(this.inputController.movementInput) * -1;

        this.rigidbody.velocity = new Vector2(wallMaterial.startJumpVelocityX * direction, wallMaterial.startJumpVelocityY);
        
        if (!AudioStorage.areSoundEffectsMuted)
        {
            var currentWallMaterial = this.inputController.wallMaterial;
            if (currentWallMaterial != null)
            {
                var randomElement = Random.Range(0, currentWallMaterial.jumpSounds.Count);
                AudioSource.PlayClipAtPoint(currentWallMaterial.jumpSounds[randomElement], this.playerTransform.position, AudioStorage.soundEffectVolume);
            }
            else
            {
                AudioSource.PlayClipAtPoint(this.jumpSound, this.playerTransform.position, AudioStorage.soundEffectVolume);
            }
        }
    }

    /// <summary>
    ///  Unused
    /// </summary>
    protected override void OnExit() {}
}
