using UnityEditor;

[CustomEditor(typeof(PlayerMovement))]
public class PlayerMovementEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var playerMovement = (PlayerMovement)target;

        playerMovement.force = EditorGUILayout.FloatField("Movement Speed", playerMovement.force);
    }
}
