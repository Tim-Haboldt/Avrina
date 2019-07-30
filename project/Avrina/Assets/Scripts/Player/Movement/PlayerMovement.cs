using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerStatus))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] public float forceOnGround;
    [SerializeField] public float forceInAir;
    [SerializeField] public float forceOnIce;
    [SerializeField] public float maxForce;
    [SerializeField] public float minForce;
    [Range(0, 1)]
    [SerializeField] public float frictionOnGround;
    [Range(0, 1)]
    [SerializeField] public float frictionInAir;
    [Range(0, 1)]
    [SerializeField] public float frictionOnIce;
    [SerializeField] public string iceTag;

    private PlayerStatus inputs;
    private Rigidbody2D rb;
    
    void Start()
    {
        this.inputs = GetComponent<PlayerStatus>();
        this.rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        var force = this.rb.velocity.x;

        switch (this.inputs.playerState)
        {
            case PlayerState.Disabled:
            case PlayerState.Uncontrolled:
                force = this.calculateFriction(force);
                break;
            case PlayerState.Mobile:
                var absForceBeforeInputs = Mathf.Abs(force);
                force = this.calculateMovementThroughGivenInputs(force);
                var absForceAfterInputs = Mathf.Abs(force);

                if (this.inputs.movementInputHorizontal == 0 || absForceBeforeInputs > this.maxForce && absForceAfterInputs > this.maxForce)
                    force = this.calculateFriction(force);

                break;
        }
        
        this.rb.velocity = new Vector2(force, this.rb.velocity.y);
    }

    private float calculateMovementThroughGivenInputs(float oldForce)
    {
        var forceSign = Mathf.Sign(oldForce);
        var absOldForce = Mathf.Abs(oldForce);
        var newForce = oldForce;

        if (this.inputs.movementInputHorizontal != 0)
        {
            // Calculate new Force due to input
            if (this.inputs.onGround)
            {
                if (this.inputs.onGroundCollider.lastColliderTag == this.iceTag)
                    newForce += this.forceOnIce * this.inputs.movementInputHorizontal;
                else
                    newForce += this.forceOnGround * this.inputs.movementInputHorizontal;
            }
            else
                newForce += this.forceInAir * this.inputs.movementInputHorizontal;
            // Remove to much force
            var absNewForce = Mathf.Abs(newForce);
            if (absOldForce > this.maxForce)
            {
                // Only allow Forces that reduce force if the force is over max already
                if (absOldForce < absNewForce)
                    newForce = oldForce;
            } else if (absNewForce > this.maxForce)
                newForce = this.maxForce * forceSign;
        }

        return newForce;
    }

    public float calculateFriction(float force)
    {
        // Apply friction dependent on the current situation
        if (this.inputs.onGround)
        {
            if (this.inputs.onGroundCollider.lastColliderTag == this.iceTag)
                force *= this.frictionOnIce;
            else
                force *= this.frictionOnGround;
        }
        else
            force *= this.frictionInAir;

        // Remove all movement if the movement is to slow
        var absNewForce = Mathf.Abs(force);
        if (absNewForce < this.minForce)
            force = 0;

        return force;
    }
}
