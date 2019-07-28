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
        var oldForce = this.rb.velocity.x;
        var absOldForce = Mathf.Abs(oldForce);
        var signOldForce = Mathf.Sign(oldForce);

        var newForce = this.calculateMovementThroughGivenInputs(oldForce, absOldForce, signOldForce);
        var absNewForce = Mathf.Abs(newForce);

        if (this.inputs.movementInputHorizontal == 0 || absOldForce > this.maxForce && absNewForce > this.maxForce)
        {
            if (this.inputs.onGround)
            {
                if (this.inputs.onGroundCollider.lastColliderTag == this.iceTag)
                    newForce *= this.frictionOnIce;
                else
                    newForce *= this.frictionOnGround;
            }
            else
                newForce *= this.frictionInAir;       

            absNewForce = Mathf.Abs(newForce);
            if (absNewForce < this.minForce)
                newForce = 0;
        }
        
        this.rb.velocity = new Vector2(newForce, this.rb.velocity.y);
    }

    private float calculateMovementThroughGivenInputs(float oldForce, float absOldForce, float signOldForce)
    {
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
                newForce = this.maxForce * signOldForce;
        }

        return newForce;
    }
}
