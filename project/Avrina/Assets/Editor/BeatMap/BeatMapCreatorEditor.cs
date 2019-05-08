using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections.Generic;

[CustomEditor(typeof(BeatMapCreator))]
public class BeatMapCreatorEditor : Editor
{
    // Used to define which file will be loaded when the corresponding button is pressed
    private int fileIndex = 0;

    public override void OnInspectorGUI()
    {
        var bmc = (BeatMapCreator)target;

        // If specific changes accoured the UI and the map data needs to be regenerated
        // Those varables define which part need to be regenerated
        bool updateUI = false;
        bool updateTickList = false;

        // Specify elements of the map
        EditorGUILayout.LabelField("Map Specifications", EditorStyles.boldLabel);
        bmc.trackTick = (AudioClip)EditorGUILayout.ObjectField("Track", bmc.trackTick, typeof(AudioClip), true);
        bmc.tickClip = (AudioClip)EditorGUILayout.ObjectField("Effect Sound", bmc.tickClip, typeof(AudioClip), true);
        var BPM = EditorGUILayout.FloatField("Track BPM", bmc.trackBPM);
        if (BPM != bmc.trackBPM) {
            updateTickList = true;
            updateUI = true;
            bmc.trackBPM = BPM;
        }
        bmc.offset = EditorGUILayout.FloatField("Track Offset", bmc.offset);
        var MTPB = EditorGUILayout.IntField("Ticks per Beat", bmc.markableTicksPerBeat);
        if (MTPB != bmc.markableTicksPerBeat)
        {
            updateTickList = true;
            updateUI = true;
            bmc.markableTicksPerBeat = MTPB;
        }

        // Customice the editor window
        EditorGUILayout.LabelField("Editor Customization", EditorStyles.boldLabel);
        var VB = EditorGUILayout.IntField("Visible Beats", bmc.visibleBeats);
        if (VB != bmc.visibleBeats)
        {
            updateUI = true;
            bmc.visibleBeats = VB;
        }
        var SOB = EditorGUILayout.Vector2Field("Size of Beat", bmc.beatSize);
        if (SOB != bmc.beatSize)
        {
            updateUI = true;
            bmc.beatSize = SOB;
        }
        var SOT = EditorGUILayout.Vector2Field("Size of Tick", bmc.tickSize);
        if (SOT != bmc.tickSize)
        {
            updateUI = true;
            bmc.tickSize = SOT;
        }
        
        // If specific changes accoured the UI and the map data needs to be regenerated
        if (updateTickList && Application.isPlaying)
            bmc.CreateTrackTicksList();
        if (updateUI && Application.isPlaying)
        {
            bmc.mapUI.DestroyUI();
            bmc.mapUI.SetupUI(bmc.visibleBeats, bmc.markableTicksPerBeat, bmc.beatSize, bmc.tickSize);
            bmc.mapUI.UpdateUI(bmc.trackRythmUnits, bmc.currentRythmUnit);

        }
        // You are only able to save or load a song if the game is currently running
        if (Application.isPlaying && bmc.enabled)
        {
            /* Save Map */
            // All elements that are used to save the map
            EditorGUILayout.LabelField("Map Manager", EditorStyles.boldLabel);
            EditorGUILayout.BeginHorizontal();
            bmc.saveName = EditorGUILayout.TextField(bmc.saveName);
            if (GUILayout.Button("Save Map"))
            {
                bmc.SaveMap();
            }
            EditorGUILayout.EndHorizontal();

            /* Load Map */
            // First get all savestates of that specific song
            var info = new DirectoryInfo(Application.persistentDataPath);
            var files = info.GetFiles();
            var fileNames = new List<string>();
            foreach (var file in files)
            {
                if (file.Name.Contains(bmc.trackSource.clip.name))
                {
                    fileNames.Add(file.Name);
                }
            }
            // Do not show the overlay if there are no maps to load
            if (fileNames.Count <= 0)
            {
                return;
            }
            // All elements that are used to load a map
            EditorGUILayout.BeginHorizontal();
            this.fileIndex = EditorGUILayout.Popup(this.fileIndex, fileNames.ToArray());
            bmc.loadName = fileNames[fileIndex];
            if (GUILayout.Button("Load Map"))
            {
                bmc.LoadMap();
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}
