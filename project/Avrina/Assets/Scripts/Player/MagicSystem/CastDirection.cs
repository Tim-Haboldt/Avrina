using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class CastDirection : MonoBehaviour
{
    /// <summary>
    ///  Stores the orientation of the arrow
    /// </summary>
    public Vector2 currentOrientation { private set; get; }
    /// <summary>
    ///  Distance of the arrow to the player
    /// </summary>
    [SerializeField] private float distanceToPlayer;

    /// <summary>
    ///  Updates the direction of the arrow
    /// </summary>
    public void UpdateDirection(Vector2 center, Vector2 orientation)
    {
        this.currentOrientation = orientation;

        var angle = Vector2.SignedAngle(Vector2.right, orientation);
        this.transform.localRotation = Quaternion.Euler(0, 0, angle);
        this.transform.position = center + orientation * distanceToPlayer;
    }
}
