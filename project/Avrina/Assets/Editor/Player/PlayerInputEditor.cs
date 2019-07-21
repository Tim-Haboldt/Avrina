using UnityEditor;

[CustomEditor(typeof(PlayerStatus))]
public class PlayerInputEditor : Editor
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
        EditorGUILayout.LabelField("Horizontal", inputs.movementInput.ToString());
        EditorGUILayout.LabelField("Jump", inputs.jumpInput.ToString());
        EditorGUILayout.LabelField("OnGround", inputs.onGround.ToString());
        EditorGUILayout.LabelField("IsSlidingWall", inputs.isSlidingTheWall.ToString());
        EditorGUILayout.LabelField("Nearest Wall", inputs.currentSlidingWallDirection.ToString());
    }
}
