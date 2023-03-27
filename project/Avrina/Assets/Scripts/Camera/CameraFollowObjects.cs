using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Camera))]
public class CameraFollowObjects : MonoBehaviour
{
    /// <summary>
    ///  Stores all objects the camera tries to follow. (Currently supports only max two objects)
    /// </summary>
    [SerializeField] private List<Transform> objectsToFollow;
    /// <summary>
    ///  What is the smallest view the camera shows
    /// </summary>
    [SerializeField] private float minZoomInFactor;
    /// <summary>
    ///  What is the fathest view before the camera splits into multiple
    /// </summary>
    [SerializeField] private float maxZoomOutFactor;
    /// <summary>
    ///  The camera will not show anything outside of the bounds
    /// </summary>
    [SerializeField] public Rect cameraBounds;
    /// <summary>
    ///  Used to smoothly zoom out on two players
    /// </summary>
    [SerializeField] public float maxOrthographUpdateRate;
    /// <summary>
    ///  Used to smoothly zoom out on two players
    /// </summary>
    [SerializeField] public float minOrthographChange;

    /// <summary>
    ///  Main Camera object
    /// </summary>
    private Camera mainCamera;
    /// <summary>
    ///  Will the height width ratio of the screen
    /// </summary>
    private float screenRation;

    /// <summary>
    ///  Will be used to make the transition smoother
    ///  Stores the oldOrthographicSize since the camera startet going bigger / smaller
    /// </summary>
    private float oldOrthographicSize;
    /// <summary>
    ///  Stres the nextOrthographic size the camera tries to ajust to
    /// </summary>
    private float nextOrthographicSize;
    /// <summary>
    ///  When did the scaling start
    /// </summary>
    private float startTime;


    /// <summary>
    ///  Initializes the objects to follow list if not already initialized
    /// </summary>
    private void Start()
    {
        if (this.objectsToFollow == null)
        {
            this.objectsToFollow = new List<Transform>();
        }

        this.mainCamera = GetComponent<Camera>();

        this.screenRation = (float)Screen.width / Screen.height;
    }

    /// <summary>
    ///  Will add an object to follow to the main camera
    /// </summary>
    /// <param name="objectToFollow">Will be added to the object which will be followed</param>
    public void AddObjectToFollow(Transform objectToFollow)
    {
        this.objectsToFollow.Add(objectToFollow);
    }

    /// <summary>
    ///  Updates the camera position in order to get all the objects inside the view
    /// </summary>
    private void LateUpdate()
    {
        switch (this.objectsToFollow.Count)
        {
            case 0:
                this.mainCamera.orthographicSize = this.minZoomInFactor;
                this.transform.position = new Vector3(0f, 0f, -5f);
                break;
            case 1:
                this.UpdateViewForSingleObject();
                break;
            default:
                this.UpdateViewForMultipleObjects();
                break;
        }
    }

    /// <summary>
    ///  Will be called if the camera follows only one object
    /// </summary>
    private void UpdateViewForSingleObject()
    {
        this.mainCamera.orthographicSize = this.minZoomInFactor;

        var vertExtent = this.mainCamera.orthographicSize;
        var horzExtent = vertExtent * this.screenRation;

        var minXPosOfCamera = this.cameraBounds.min.x + horzExtent;
        var maxXPosOfCamera = this.cameraBounds.max.x - horzExtent;
        var minYPosOfCamera = this.cameraBounds.min.y - vertExtent;
        var maxYPosOfCamera = this.cameraBounds.max.y + vertExtent;

        var objectPos = this.objectsToFollow[0].position;
        objectPos.x = Mathf.Clamp(objectPos.x, minXPosOfCamera, maxXPosOfCamera);
        objectPos.y = Mathf.Clamp(objectPos.y, maxYPosOfCamera, minYPosOfCamera);
        objectPos.z = -5f;
        this.transform.position = objectPos;
    }

    /// <summary>
    ///  Will be called if the camera follows multiple objects
    /// </summary>
    private void UpdateViewForMultipleObjects()
    {
        // Get camera size
        var vertExtent = this.minZoomInFactor;
        var horzExtent = vertExtent * this.screenRation;
        // Calculate true camera bounds
        var minXPosOfCamera = this.cameraBounds.min.x + horzExtent;
        var maxXPosOfCamera = this.cameraBounds.max.x - horzExtent;
        var minYPosOfCamera = this.cameraBounds.min.y - vertExtent;
        var maxYPosOfCamera = this.cameraBounds.max.y + vertExtent;
        // Apply camera bounds to the first object in order to ignore edge movement
        var objectOnePos = this.objectsToFollow[0].position;
        objectOnePos.x = Mathf.Clamp(objectOnePos.x, minXPosOfCamera, maxXPosOfCamera);
        objectOnePos.y = Mathf.Clamp(objectOnePos.y, maxYPosOfCamera, minYPosOfCamera);
        // Apply camera bounds to the second object in order to ignore edge movement
        var objectTwoPos = this.objectsToFollow[1].position;
        objectTwoPos.x = Mathf.Clamp(objectTwoPos.x, minXPosOfCamera, maxXPosOfCamera);
        objectTwoPos.y = Mathf.Clamp(objectTwoPos.y, maxYPosOfCamera, minYPosOfCamera);
        // Get the distance between the objects
        var distanceX = Mathf.Abs(objectTwoPos.x - objectOnePos.x) * 0.5f + horzExtent;
        var distanceY = Mathf.Abs(objectTwoPos.y - objectOnePos.y) * 0.5f + vertExtent;
        // Calculate the orthographic number for the camera component
        var orthographicSize = distanceY;
        if (orthographicSize * this.screenRation < distanceX)
        {
            orthographicSize = distanceX / this.screenRation;
        }
        // Only apply the min orthographic number for the camera
        if (orthographicSize < this.minZoomInFactor)
        {
            orthographicSize = this.minZoomInFactor;
        }
        // Only apply the max orthographic number for the camera and handle bigger distances differently
        if (orthographicSize > this.maxZoomOutFactor)
        {
            orthographicSize = this.maxZoomOutFactor;
            // Map object two position to one inside the camera
            // Spawn second camera for object player
            this.SpawnSecondCamera(minXPosOfCamera, maxXPosOfCamera, minYPosOfCamera, maxYPosOfCamera);
        }
        // Only update the orthographic size if the threashhold was passed
        var orthographDifferenze = orthographicSize - this.mainCamera.orthographicSize;
        if (this.minOrthographChange < Mathf.Abs(orthographDifferenze))
        {
            // Update the orthographic size only slowly
            if (this.maxOrthographUpdateRate < Mathf.Abs(orthographDifferenze))
            {
                var orthographSign = Mathf.Sign(orthographDifferenze);
                orthographicSize = this.mainCamera.orthographicSize + orthographSign * this.maxOrthographUpdateRate;
            }
        }
        else
        {
            orthographicSize = this.mainCamera.orthographicSize;
        }
        // Apply orthographic size to the camera
        this.mainCamera.orthographicSize = orthographicSize;
        // Update player position to the true one
        objectOnePos = this.objectsToFollow[0].position;
        objectTwoPos = this.objectsToFollow[1].position;
        //calculate camera position between the two player objects
        var newCameraPos = new Vector3(
            objectOnePos.x + (objectTwoPos.x - objectOnePos.x) * 0.5f,
            objectOnePos.y + (objectTwoPos.y - objectOnePos.y) * 0.5f,
            -5f
        );
        // Calculate new camera bounds
        vertExtent = orthographicSize;
        horzExtent = vertExtent * this.screenRation;
        minXPosOfCamera = this.cameraBounds.min.x + horzExtent;
        maxXPosOfCamera = this.cameraBounds.max.x - horzExtent;
        minYPosOfCamera = this.cameraBounds.min.y - vertExtent;
        maxYPosOfCamera = this.cameraBounds.max.y + vertExtent;
        // Adjust camera position
        newCameraPos.x = Mathf.Clamp(newCameraPos.x, minXPosOfCamera, maxXPosOfCamera);
        newCameraPos.y = Mathf.Clamp(newCameraPos.y, maxYPosOfCamera, minYPosOfCamera);
        // Apply new position
        this.transform.position = newCameraPos;
    }

    /// <summary>
    ///  Spawns the second camera for the second object if both objects are to far from each other
    /// </summary>
    private void SpawnSecondCamera(float minXPosOfCamera, float maxXPosOfCamera, float minYPosOfCamera, float maxYPosOfCamera)
    {

    }
}
