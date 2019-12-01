using UnityEngine;

public class AirJump : SpellBase
{
    [SerializeField] public float force;
    [SerializeField] public float maxAngle;

    public override Element firstElement { get { return Element.Earth; } }
    public override Element secondElement { get { return Element.Air; } }

    public override void CastSpell(GameObject player, Vector2 castDirection)
    {
        var playerRigidBody = player.GetComponent<Rigidbody2D>();
        var normalizedCastDir = castDirection.normalized;

        // Do not allow every angle
        var castDirAsAngle = Vector2.SignedAngle(Vector2.up, normalizedCastDir);
        if (Mathf.Abs(castDirAsAngle) > maxAngle)
        {
            var signOfAngle = Mathf.Sign(castDirAsAngle);
            var reducedAngle = signOfAngle * maxAngle;
            // The angle is seen in relation to Vector2.up and needs to be rotated to the scope of Vector2.right
            reducedAngle += 90;
            // Degree to Vector2
            normalizedCastDir = new Vector2(Mathf.Cos(reducedAngle), Mathf.Sign(reducedAngle)).normalized;
        }

        playerRigidBody.velocity = normalizedCastDir * force;
    }
}
