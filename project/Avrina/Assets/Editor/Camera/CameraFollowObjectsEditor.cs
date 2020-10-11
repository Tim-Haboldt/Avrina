using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CameraFollowObjects))]
public class CameraFollowObjectsEditor : Editor
{
    /// <summary>
    ///  Is the camera bounding box currenlty being edited
    /// </summary>
    private bool currentlyEditing = false;
    /// <summary>
    ///  Current camera bounding box
    /// </summary>
    private Rect currentBoudingBox;
    /// <summary>
    ///  Stores the current target script component instance
    /// </summary>
    private CameraFollowObjects cameraScript;


    /// <summary>
    ///  Stores the target script component
    /// </summary>
    private void OnEnable()
    {
        this.cameraScript = (CameraFollowObjects) target;
    }

    /// <summary>
    ///  Adds an edit and save button for the camera bounding box
    /// </summary>
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Show"))
        {
            this.currentBoudingBox = this.cameraScript.cameraBounds;
            this.currentlyEditing = true;

            this.Redraw();
        }
        if (this.currentlyEditing)
        {
            if (GUILayout.Button("Hide"))
            {
                this.currentlyEditing = false;
                this.Redraw();
            }
        }
        GUILayout.EndHorizontal();
    }

    /// <summary>
    ///  Will update the scene gui
    /// </summary>
    public void Redraw()
    {
        var view = EditorWindow.GetWindow<SceneView>();
        view.Repaint();
    }

    /// <summary>
    ///  Will call the camera bounds and handle the inputs
    /// </summary>
    private void OnSceneGUI()
    {
        if (!this.currentlyEditing)
        {
            return;
        }
        
        this.DrawBoundingBox();
    }

    /// <summary>
    ///  Used to draw the camera bounding box
    /// </summary>
    private void DrawBoundingBox()
    {
        Vector3[] outlinePoints =
        {
            currentBoudingBox.min,
            new Vector3(currentBoudingBox.xMin, currentBoudingBox.yMax),
            currentBoudingBox.max,
            new Vector3(currentBoudingBox.xMax, currentBoudingBox.yMin)
        };
        int[] indicies =
        {
            0, 1,
            1, 2,
            2, 3,
            3, 0
        };

        Handles.DrawLines(outlinePoints, indicies);
    }
}
