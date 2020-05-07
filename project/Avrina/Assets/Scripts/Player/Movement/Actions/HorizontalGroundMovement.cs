﻿using UnityEngine;

public class HorizontalGroundMovement : Action
{
    /// <summary>
    ///  What is the mask of the ground objects
    /// </summary>
    private LayerMask groundMask;


    /**
     * <summary>
     *  Will execute the action.
     * </summary>
     * <example>
     *  Apply gravity to player
     * </example>
     * <param name="velocity">Used to modify the velocity of the player</param>
     */
    public void PerformAction(ref Vector2 velocity, PlayerController playerController)
    {
        var movementInput = playerController.movementInput;
        var groundMaterial = playerController.groundMaterial;
        if (groundMaterial == null)
        {
            return;
        }

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
            var playerBottom = this.getCenterBottomOfPlayer(playerController);
            RaycastHit2D hit = Physics2D.Raycast(playerBottom, Vector2.down, 5, this.groundMask);
            if (hit.collider != null)
            {
                var slopeAngle = Vector2.SignedAngle(hit.normal, Vector2.up);
                var velocityX = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * nextMovementSpeed;
                var velocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * nextMovementSpeed;

                velocityY *= nextMovementSpeed / velocityX;
                Debug.Log(velocity);

                velocity = new Vector2(nextMovementSpeed, -velocityY);
            }
        }
    }

    /// <summary>
    ///  Returns the middle bottom position of the player
    /// </summary>
    /// <param name="controller"></param>
    /// <returns></returns>
    private Vector2 getCenterBottomOfPlayer(PlayerController controller)
    {
        var capsuleCollider = controller.transform.gameObject.GetComponent<CapsuleCollider2D>();
        var position = capsuleCollider.bounds.center;
        var halfSizeY = capsuleCollider.bounds.size.y * 0.5f;

        return new Vector2(position.x, position.y - halfSizeY);
    }

    /**
     * <summary>
     *  Reads the constants from the config
     * </summary>
     */
    public void Setup(PlayerConfig config)
    {
        this.groundMask = config.groundMask;
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
