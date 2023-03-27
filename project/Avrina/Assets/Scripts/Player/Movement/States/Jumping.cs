using UnityEngine;

public class Jumping : StateInheritingAction
{
    /// <summary>
    ///  Start time of the jump
    /// </summary>
    private float startTime;
    /// <summary>
    ///  What is the maximal jump duration
    /// </summary>
    public float maxJumpDuration;
    /// <summary>
    ///  What is the air jump velocity. Will be used if the groundMask is unknown
    /// </summary>
    private float airJumpVelocity;
    /// <summary>
    ///  What is the gravity while the player is jumping
    /// </summary>
    private float gravityDuringJump;

    /// <summary>
    ///  Name of the state is Jumping
    /// </summary>
    public override PlayerState name { get; } = PlayerState.Jumping;
    /// <summary>
    ///  The player can jump and move horizontal while jumping
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
    ///  Caluclates the current jump velocity
    /// </summary>
    /// <param name="velocity"></param>
    protected override void PerformAction(ref Vector2 velocity)
    {
        velocity = new Vector2(velocity.x, velocity.y - this.gravityDuringJump);
    }

    /// <summary>
    ///  Keeps returning the jump state until the jump button is not pressed or the maximal jumpduration was exceeded
    /// </summary>
    /// <returns>Returns the State InAir or Jumping</returns>
    public override PlayerState Update()
    {
        var passedTime = Time.time - this.startTime;

        if (!this.inputController.jumpInput || passedTime >= this.maxJumpDuration)
        {
            return PlayerState.InAir;
        }

        return this.name;
    }

    /**
    * <summary>
    *  Reads the constants from the config
    * </summary>
    */
    protected override void Setup(PlayerConfig config)
    {
        this.maxJumpDuration = config.maxJumpDuration;
        this.airJumpVelocity = config.airJumpStartVelocity;
        this.gravityDuringJump = config.gravityDuringJump;
        this.jumpSound = config.jumpSound;
    }

    /**
     * <summary>
     *  Sets the start time of the jump
     * </summary>
     */
    protected override void OnEnter()
    {
        this.startTime = Time.time;
        if (this.inputController.groundMaterial == null)
        {
            this.rigidbody.velocity = new Vector2(rigidbody.velocity.x, this.airJumpVelocity);
        }
        else
        {
            this.rigidbody.velocity = new Vector2(rigidbody.velocity.x, this.inputController.groundMaterial.jumpVelocity);
        }

        if (!AudioStorage.areSoundEffectsMuted)
        {
            var currentGroundMaterial = this.inputController.groundMaterial;
            if (currentGroundMaterial != null)
            {
                var randomElement = Random.Range(0, currentGroundMaterial.jumpSounds.Count);
                AudioSource.PlayClipAtPoint(currentGroundMaterial.jumpSounds[randomElement], this.playerTransform.position, AudioStorage.soundEffectVolume * 0.5f);
            }
            else
            {
                AudioSource.PlayClipAtPoint(this.jumpSound, this.playerTransform.position, AudioStorage.soundEffectVolume * 0.5f);
            }
        }
    }

    /**
     * <summary>
     *  Unused
     * </summary>
     */
    protected override void OnExit() { }
}
