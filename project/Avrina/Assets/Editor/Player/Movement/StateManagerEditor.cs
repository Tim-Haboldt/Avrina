using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerStateManager))]
public class StateManagerEditor : Editor
{
    /// <summary>
    ///  Target object all changes will be applied to
    /// </summary>
    PlayerStateManager manager;

    /**
     * <summary>
     *  Will be called when the script is enabled.
     *  Gets the target object.
     * </summary>
     */
    private void OnEnable()
    {
        this.manager = (PlayerStateManager)target;
    }

    /**
     * <summary>
     *  Add a new button to the inspector which updates the states with a new player config
     * </summary>
     */
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Apply Config"))
        {
            this.manager.ApplyConfig();
        }
    }
}
