using UnityEngine;

public class CameraFollowObject : MonoBehaviour
{
    public Transform objectToFollow;
    public float yDistance = -5f;
    
    void LateUpdate()
    {
        this.transform.position = new Vector3(objectToFollow.position.x, objectToFollow.position.y, yDistance);
    }
}
