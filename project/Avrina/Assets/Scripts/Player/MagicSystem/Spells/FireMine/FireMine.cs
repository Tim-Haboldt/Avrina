using UnityEngine;
using Mirror;

[RequireComponent(typeof(Animator))]
public class FireMine : Spell
{
    private enum FireMineState
    {
        EXPLODING,
        IDLE
    }

    /// <summary>
    ///  Idle Animation
    /// </summary>
    [SerializeField] private RuntimeAnimatorController idleAnimatior = null;
    /// <summary>
    ///  How long will the i
    /// </summary>
    [SerializeField] private float idleAnimationLength = 0f;
    /// <summary>
    ///  Explosion Animation
    /// </summary>
    [SerializeField] private RuntimeAnimatorController explosionAnimatior = null;
    /// <summary>
    ///  
    /// </summary>
    [SerializeField] private float explosionAnimationLength = 0f;

    /// <summary>
    ///  Stores the animator instance
    /// </summary>
    private Animator animator = null;
    /// <summary>
    ///  Start time of the animation
    /// </summary>
    private float startTime;
    /// <summary>
    ///  Stores the current state of the fire mine
    /// </summary>
    private FireMineState currentState = FireMineState.IDLE;

    
    /// <summary>
    ///  Handles the animations
    /// </summary>
    private void Update()
    {
        switch (this.currentState) {
            case FireMineState.IDLE:
                if (this.startTime + this.idleAnimationLength < Time.time)
                {
                    this.ChangeAnimationOnClient();
                    this.startTime += this.idleAnimationLength;
                    this.currentState = FireMineState.EXPLODING;
                }
                break;
            case FireMineState.EXPLODING:
                if (this.startTime + this.explosionAnimationLength < Time.time)
                {
                    Destroy(this.gameObject);
                }
                break;
        }
    }

    /// <summary>
    ///  Changes the animation but only on the client
    /// </summary>
    [Client]
    private void ChangeAnimationOnClient()
    {
        this.animator.runtimeAnimatorController = this.explosionAnimatior;
    }

    /// <summary>
    ///  Returns the player position 
    /// </summary>
    /// <param name="playerPosition"></param>
    /// <param name="castDirection"></param>
    /// <returns></returns>
    protected override Vector2 CalculateStartPosition(Vector2 playerPosition, Vector2 castDirection)
    {
        return playerPosition;
    }

    /// <summary>
    ///  Will be called when the server starts
    /// </summary>
    protected override void HandleServerStart()
    {
        this.startTime = Time.time;
    }

    /// <summary>
    ///  Will be called on the client start
    /// </summary>
    protected override void HandleClientStart()
    {
        this.animator = GetComponent<Animator>();
        this.animator.runtimeAnimatorController = this.idleAnimatior;
        this.startTime = Time.time;
    }
}
