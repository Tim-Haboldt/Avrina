using UnityEditor;

[CustomEditor(typeof(PlayerController))]
public class PlayerControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.LabelField("Horizontal", PlayerController.movementInput.ToString());
        EditorGUILayout.LabelField("Jump", PlayerController.jumpInput.ToString());
        EditorGUILayout.LabelField("OnGround", PlayerController.onGround.ToString());
        EditorGUILayout.LabelField("OnWallLeft", PlayerController.hasWallLeft.ToString());
        EditorGUILayout.LabelField("OnWallRight", PlayerController.hasWallRight.ToString());
    }
}
