using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections.Generic;

[CustomEditor(typeof(BeatMapCreator))]
public class BeatMapCreatorEditor : Editor
{
    // used to define which file will be loaded when the corresponding button is pressed
    private int fileIndex = 0;

    public override void OnInspectorGUI()
    {
        BeatMapCreator bmc = (BeatMapCreator)target;

        // if specific changes accoured the UI and the map data needs to be regenerated
        // those varables define which part need to be regenerated
        bool updateUI = false;
        bool updateTickList = false;

        // specify elements of the map
        EditorGUILayout.LabelField("Map Specifications", EditorStyles.boldLabel);
        bmc.trackSource = (AudioClip)EditorGUILayout.ObjectField("Track", bmc.trackSource, typeof(AudioClip), true);
        bmc.tickSource = (AudioClip)EditorGUILayout.ObjectField("Effect Sound", bmc.tickSource, typeof(AudioClip), true);
        float BPM = EditorGUILayout.FloatField("Track BPM", bmc.trackBPM);
        if (BPM != bmc.trackBPM) {
            updateTickList = true;
            updateUI = true;
            bmc.trackBPM = BPM;
        }
        bmc.offset = EditorGUILayout.FloatField("Track Offset", bmc.offset);
        int MTPB = EditorGUILayout.IntField("Ticks per Beat", bmc.markableTicksPerBeat);
        if (MTPB != bmc.markableTicksPerBeat)
        {
            updateTickList = true;
            updateUI = true;
            bmc.markableTicksPerBeat = MTPB;
        }

        // customice the editor window
        EditorGUILayout.LabelField("Editor Customization", EditorStyles.boldLabel);
        int VB = EditorGUILayout.IntField("Visible Beats", bmc.visibleBeats);
        if (VB != bmc.visibleBeats)
        {
            updateUI = true;
            bmc.visibleBeats = VB;
        }
        Vector2 SOB = EditorGUILayout.Vector2Field("Size of Beat", bmc.beatSize);
        if (SOB != bmc.beatSize)
        {
            updateUI = true;
            bmc.beatSize = SOB;
        }
        Vector2 SOT = EditorGUILayout.Vector2Field("Size of Tick", bmc.tickSize);
        if (SOT != bmc.tickSize)
        {
            updateUI = true;
            bmc.tickSize = SOT;
        }
        
        // if specific changes accoured the UI and the map data needs to be regenerated
        if (updateTickList && Application.isPlaying)
            bmc.createTrackTicksList();
        if (updateUI && Application.isPlaying)
        {
            bmc.beatMapUI.destroyUI();
            bmc.beatMapUI.setupUI(bmc.visibleBeats, bmc.markableTicksPerBeat, bmc.beatSize, bmc.tickSize);
            bmc.beatMapUI.updateUI(bmc.trackTicks, bmc.currentTick, bmc.visibleBeats, bmc.markableTicksPerBeat);

        }

        // all elements that are used to save the map
        EditorGUILayout.LabelField("Map Manager", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();
        bmc.saveName = EditorGUILayout.TextField(bmc.saveName);
        if (GUILayout.Button("Save Map"))
        {
            bmc.saveMap();
        }
        EditorGUILayout.EndHorizontal();

        // you are only able to load a song if the game is currently running
        if (Application.isPlaying)
        {
            // first get all savestates of that specific song
            DirectoryInfo info = new DirectoryInfo(Application.persistentDataPath);
            FileInfo[] files = info.GetFiles();
            List<string> fileNames = new List<string>();
            foreach (var file in files)
            {
                if (file.Name.Contains(bmc.track.clip.name))
                    fileNames.Add(file.Name);
            }
            // do not show the overlay if there are no maps to load
            if (fileNames.Count <= 0)
                return;
            // all elements that are used to load a map
            EditorGUILayout.BeginHorizontal();
            this.fileIndex = EditorGUILayout.Popup(this.fileIndex, fileNames.ToArray());
            bmc.loadName = fileNames[fileIndex];
            if (GUILayout.Button("Load Map"))
            {
                bmc.loadMap();
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}
