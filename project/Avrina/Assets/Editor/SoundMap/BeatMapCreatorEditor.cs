using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BeatMapCreator))]
public class BeatMapCreatorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        BeatMapCreator bmc = (BeatMapCreator)target;

        // specify elements of the map
        EditorGUILayout.LabelField("Map Specifications", EditorStyles.boldLabel);
        bmc.trackSource = (AudioClip)EditorGUILayout.ObjectField("Track", bmc.trackSource, typeof(AudioClip), true);
        bmc.tickSource = (AudioClip)EditorGUILayout.ObjectField("Effect Sound", bmc.tickSource, typeof(AudioClip), true);
        bmc.trackBPM = EditorGUILayout.FloatField("Track BPM", bmc.trackBPM);
        bmc.offset = EditorGUILayout.FloatField("Track Offset", bmc.offset);
        bmc.markableTicksPerBeat = EditorGUILayout.IntField("Ticks per Beat", bmc.markableTicksPerBeat);

        // customice the editor window
        EditorGUILayout.LabelField("Editor Customization", EditorStyles.boldLabel);
        bmc.visibleKeyBeats = EditorGUILayout.IntField("Visible Beats", bmc.visibleKeyBeats);
        bmc.beatSize = EditorGUILayout.Vector2Field("Size of Beat", bmc.beatSize);
        bmc.tickSize = EditorGUILayout.Vector2Field("Size of Tick", bmc.tickSize);
    }
}
