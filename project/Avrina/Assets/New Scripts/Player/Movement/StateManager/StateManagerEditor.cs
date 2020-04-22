using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(StateManager))]
public class StateManagerEditor : Editor
{
    /// <summary>
    ///  Target object all changes will be applied to
    /// </summary>
    StateManager manager;

    /**
     * <summary>
     *  Will be called when the script is enabled.
     *  Gets the target object.
     * </summary>
     */
    private void OnEnable()
    {
        this.manager = (StateManager)target;
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
