using UnityEditor;

[CustomEditor(typeof(PlayerMovement))]
public class PlayerMovementEditor : Editor
{
    public override void OnInspectorGUI()
    {
        PlayerMovement playerMovement = (PlayerMovement)target;

        playerMovement.speed = EditorGUILayout.FloatField("Movement Speed", playerMovement.speed);
    }
}
