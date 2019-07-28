using UnityEditor;

[CustomEditor(typeof(MagicSystemStatus))]
public class MagicSystemStatusEditor : Editor
{
    private MagicSystemStatus status;

    private void OnEnable()
    {
        if (this.status == null)
            this.status = (MagicSystemStatus)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.LabelField("Is First Direction Selected", status.isFirstDirectionSelected.ToString());
        EditorGUILayout.LabelField("Is Second Direction Selected", status.isSecondDirectionSelected.ToString());
        EditorGUILayout.LabelField("Is Casting Spell", status.isPlayerCastingSpell.ToString());
    }
}
