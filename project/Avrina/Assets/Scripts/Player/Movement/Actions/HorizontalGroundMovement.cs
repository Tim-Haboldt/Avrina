using UnityEngine;

public class HorizontalGroundMovement : Action
{
    /// <summary>
    ///  What is the mask of the ground objects
    /// </summary>
    private LayerMask groundMask;
    /// <summary>
    ///  Defines how fast a slope can fall down for the player to still act as if on ground
    /// </summary>
    private float maxGroundDistanceForSlopes;
    /// <summary>
    ///  Used to get the current player position
    /// </summary>
    private Transform playerTransform;
    /// <summary>
    ///  Used to play the movement sound effect
    /// </summary>
    private Vector3 lastMovementSoundPosition;


    /**
     * <summary>
     *  Will execute the action.
     * </summary>
     * <example>
     *  Apply gravity to player
     * </example>
     * <param name="velocity">Used to modify the velocity of the player</param>
     */
    public void PerformAction(ref Vector2 velocity, InputController inputController)
    {
        var movementInput = inputController.movementInput;
        var groundMaterial = inputController.groundMaterial;
        if (groundMaterial == null)
        {
            return;
        }

        this.PlayWalkSoundEffect(inputController);

        var absoluteMovementSpeed = Mathf.Sqrt(Mathf.Pow(velocity.x, 2) + Mathf.Pow(velocity.y, 2));
        var nextMovementSpeed = absoluteMovementSpeed * Mathf.Sign(velocity.x);

        if (movementInput == 0)
        {
            nextMovementSpeed -= groundMaterial.frictionWhileNoInputGiven * nextMovementSpeed;

            if (Mathf.Abs(nextMovementSpeed) < groundMaterial.smallestMovementBeforeStop)
            {
                nextMovementSpeed = 0;
            }
        }
        else if (Mathf.Sign(movementInput) == Mathf.Sign(nextMovementSpeed))
        {
            nextMovementSpeed += movementInput * groundMaterial.acceleration - groundMaterial.frictionWhileMoving * nextMovementSpeed;
        }
        else
        {
            nextMovementSpeed += movementInput * groundMaterial.acceleration - groundMaterial.frictionWhileTurning * nextMovementSpeed;
        }

        if (nextMovementSpeed == 0)
        {
            velocity = new Vector2(0, 0);
        }
        else
        {
            // Do not change from ground to air state if the last ground position height was close to the same as the next one even if the player is in the air.
            var playerBottom = this.GetCenterOfPlayer(inputController, Vector2.down, 0.5f);
            RaycastHit2D ground = Physics2D.Raycast(playerBottom, Vector2.down, this.maxGroundDistanceForSlopes, this.groundMask);
            var slopeAngle = 0f;
            if (ground.collider != null)
            {
                slopeAngle = Vector2.SignedAngle(ground.normal, Vector2.up);
            }

            var velocityX = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * nextMovementSpeed;
            var velocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * nextMovementSpeed;

            if (velocityY < 0)
            {
                // The player is moving up a slope
                var movementDirection = new Vector2(Mathf.Sign(nextMovementSpeed), 0);
                var corner = this.GetCenterOfPlayer(inputController, movementDirection, 0.5f);
                RaycastHit2D nextGround = Physics2D.Raycast(corner, Vector2.down, this.maxGroundDistanceForSlopes, this.groundMask);
                if (nextGround.collider != null)
                {
                    if (Vector2.SignedAngle(nextGround.normal, Vector2.up) == 0)
                    {
                        velocity = new Vector2(nextMovementSpeed, 0);
                        return;
                    }
                }
            }

            velocity = new Vector2(velocityX, -velocityY);
        }
    }

    /// <summary>
    ///  Will use the current player position and the old sound played position to calculate the next point a movement sound should be played
    /// </summary>
    private void PlayWalkSoundEffect(InputController inputController)
    {
        var currentGroundMaterial = inputController.groundMaterial;
        if (!AudioStorage.areSoundEffectsMuted && currentGroundMaterial != null)
        {
            if (this.lastMovementSoundPosition != null)
            {
                var distance = Vector3.Distance(this.lastMovementSoundPosition, this.playerTransform.position);
                if (distance < 2.7f)
                {
                    return;
                }
            }

            var randomElement = Random.Range(0, currentGroundMaterial.walkSounds.Count);
            AudioSource.PlayClipAtPoint(currentGroundMaterial.walkSounds[randomElement], this.playerTransform.position, AudioStorage.soundEffectVolume * 0.7f);

            this.lastMovementSoundPosition = this.playerTransform.position;
        }
    }

    /// <summary>
    ///  Returns the center of the player with an offset in any given direction
    /// </summary>
    /// <param name="controller">Used to get the center of the player</param>
    /// <param name="direction">What is the direction of the offset</param>
    /// <param name="offset">How much is the center offset in the given direction</param>
    /// <returns></returns>
    private Vector2 GetCenterOfPlayer(InputController controller, Vector2 direction, float offset)
    {
        var capsuleCollider = controller.playerCollider;
        var position = capsuleCollider.bounds.center;
        var halfSizeX = capsuleCollider.bounds.size.x * offset;
        var halfSizeY = capsuleCollider.bounds.size.x * offset;

        return new Vector2(position.x + halfSizeX * direction.x, position.y + halfSizeY * direction.y);
    }

    /**
     * <summary>
     *  Reads the constants from the config
     * </summary>
     */
    public void Setup(PlayerConfig config, Transform playerTransform)
    {
        this.groundMask = config.groundMask;
        this.maxGroundDistanceForSlopes = config.maxGroundDistanceForSlopes;
        this.playerTransform = playerTransform;
    }

    /**
     * <summary>
     *  Unused
     * </summary>
     */
    public void OnEnter() {}

    /**
     * <summary>
     *  Unused
     * </summary>
     */
    public void OnExit() { }
}
