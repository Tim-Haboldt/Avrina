using UnityEditor;

[CustomEditor(typeof(PlayerInput))]
public class PlayerInputEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var inputs = (PlayerInput)target;

        EditorGUILayout.LabelField("Horizontal", inputs.movementInput.ToString());
        EditorGUILayout.LabelField("Jump", inputs.jumpInput.ToString());
    }
}
