using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BeatMapCreator))]
public class BeatMapCreatorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        BeatMapCreator bmc = (BeatMapCreator)target;

        bmc.trackSource = (AudioClip)EditorGUILayout.ObjectField("Track", bmc.trackSource, typeof(AudioClip), true);
        bmc.tickSource = (AudioClip)EditorGUILayout.ObjectField("Effect Sound", bmc.tickSource, typeof(AudioClip), true);
        bmc.trackBPM = EditorGUILayout.FloatField("Track BPM", bmc.trackBPM);
        bmc.offset = EditorGUILayout.FloatField("Track Offset", bmc.offset);
        bmc.markableTicksPerBeat = EditorGUILayout.IntField("Ticks per Beat", bmc.markableTicksPerBeat);

        bmc.beatMapUI = (Canvas)EditorGUILayout.ObjectField("UI", bmc.beatMapUI, typeof(Canvas), true);
    }
}
