using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CameraFollowObject))]
public class CameraFollowObjectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        CameraFollowObject cameraFollowObject = (CameraFollowObject)target;

        cameraFollowObject.objectToFollow = (Transform)EditorGUILayout.ObjectField("Following Object", cameraFollowObject.objectToFollow, typeof(Transform), true);
        cameraFollowObject.yDistance = EditorGUILayout.FloatField("Plane Distance", cameraFollowObject.yDistance);
    }
}
