using UnityEditor;

[CustomEditor(typeof(PlayerStatus))]
public class PlayerStatusEditor : Editor
{
    private PlayerStatus inputs;

    private void OnEnable()
    {
        if (this.inputs == null)
            this.inputs = (PlayerStatus)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.LabelField("Horizontal", inputs.movementInputHorizontal.ToString());
        EditorGUILayout.LabelField("Vertical", inputs.movementInputVertical.ToString());
        EditorGUILayout.LabelField("Jump", inputs.jumpInput.ToString());
        EditorGUILayout.LabelField("OnGround", inputs.onGround.ToString());
        EditorGUILayout.LabelField("IsSlidingWall", inputs.isSlidingTheWall.ToString());
        EditorGUILayout.LabelField("Nearest Wall", inputs.currentSlidingWallDirection.ToString());
        EditorGUILayout.LabelField("Player State", inputs.playerState.ToString());
        EditorGUILayout.LabelField("Jump State", inputs.jumpState.ToString());
    }
}
