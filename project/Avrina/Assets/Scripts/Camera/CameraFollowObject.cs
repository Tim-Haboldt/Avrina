using UnityEngine;

public class CameraFollowObject : MonoBehaviour
{
    public Transform objectToFollow;
    public float yDistance = -5f;
    
    void LateUpdate()
    {
        this.transform.position = new Vector3(objectToFollow.position.x + objectToFollow.localScale.x / 2, objectToFollow.position.y - objectToFollow.localScale.y / 2, yDistance);
    }
}
