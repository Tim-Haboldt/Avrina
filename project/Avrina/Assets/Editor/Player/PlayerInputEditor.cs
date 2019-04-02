using UnityEditor;

[CustomEditor(typeof(PlayerInput))]
public class PlayerInputEditor : Editor
{
    public override void OnInspectorGUI()
    {
        PlayerInput inputs = (PlayerInput)target;

        EditorGUILayout.FloatField("Horizontal", inputs.movementInput.x);
        EditorGUILayout.FloatField("Vertical", inputs.movementInput.y);
    }
}
