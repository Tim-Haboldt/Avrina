//using System.Collections.Generic;
//using UnityEditor;
//using UnityEngine;

//[CustomEditor(typeof(PlayerStateManager))]
//public class PlayerStateManagerEditor : Editor
//{
//    private PlayerStateManager stateManager;
//    private List<bool> showStateContent;
//    private bool showPlayerStates;
    
//    private void OnEnable()
//    {
//        // Init all Objects if their not initialized
//        if (this.stateManager == null)
//            this.stateManager = (PlayerStateManager)target;

//        if (this.stateManager.selectedPlayerState == null)
//            this.stateManager.selectedPlayerState = new List<PlayerState>();

//        if (this.stateManager.actionFunctions == null)
//            this.stateManager.actionFunctions = new List<List<PlayerMovement>>();

//        if (this.stateManager.exitConditions == null)
//        {
//            this.stateManager.exitConditions = new List<List<PlayerMovement>>();
//        }

//        if (this.showStateContent == null)
//            this.showStateContent = new List<bool>();
//        for (int i = 0; i < this.stateManager.selectedPlayerState.Count; i++)
//            this.showStateContent.Add(false);

//        this.stateManager.playerState = PlayerState.InAir;
//    }

//    public override void OnInspectorGUI()
//    {
//        base.OnInspectorGUI();

//        EditorGUILayout.LabelField("Player state", this.stateManager.playerState.ToString());

//        // Create the GUIStyles and define the text colors
//        var style = new GUIStyle(GUI.skin.button);
//        style.normal.textColor = Color.green;

//        GUILayout.BeginHorizontal();
//        this.showPlayerStates = EditorGUILayout.Foldout(this.showPlayerStates, "State Actions and Exitconditions");

//        for (int space = 0; space < 7; space++)
//            EditorGUILayout.Space();

//        // Create new Elements inside the lists
//        if (GUILayout.Button("+", style))
//        {
//            this.stateManager.actionFunctions.Add(new EventFunction());
//            this.stateManager.exitConditions.Add(new EventFunction());
//            this.stateManager.selectedPlayerState.Add(PlayerState.InAir);
//            this.showStateContent.Add(true);
//        }
//        GUILayout.EndHorizontal();

//        if (this.showPlayerStates)
//        {
//            for (int i = 0; i < this.stateManager.selectedPlayerState.Count; i++)
//            {
//                EditorGUILayout.BeginVertical();

//                EditorGUILayout.BeginHorizontal();

//                for (int space = 0; space < 7; space++)
//                    EditorGUILayout.Space();

//                this.showStateContent[i] = EditorGUILayout.Foldout(this.showStateContent[i], "");

//                // Draw Section Name
//                this.stateManager.selectedPlayerState[i] = (PlayerState)EditorGUILayout.EnumPopup(this.stateManager.selectedPlayerState[i]);

//                for (int space = 0; space < 40; space++)
//                    EditorGUILayout.Space();

//                // Draw Button
//                // Remove an entry
//                style.normal.textColor = Color.red;
//                if (GUILayout.Button("x", style))
//                {
//                    this.stateManager.actionFunctions.RemoveAt(i);
//                    this.stateManager.exitConditions.RemoveAt(i);
//                    this.stateManager.selectedPlayerState.RemoveAt(i);
//                    this.showStateContent.RemoveAt(i);
//                    return;
//                }

//                EditorGUILayout.EndHorizontal();

//                EditorGUILayout.BeginHorizontal();

//                for (int space = 0; space < 2; space++)
//                    EditorGUILayout.Space();

//                if (this.showStateContent[i])
//                {
//                    // Draw Section Content
//                    EditorGUILayout.BeginVertical();
//                    // Draw the state exit conditions
//                    EditorGUILayout.LabelField("State Exit Conditions");
//                    SerializedProperty exitConditions = serializedObject.FindProperty("exitConditions");
//                    EditorGUILayout.PropertyField(exitConditions.GetArrayElementAtIndex(i));
//                    // Draw Unity Event
//                    EditorGUILayout.LabelField("State Actions");
//                    SerializedProperty actionFunctions = serializedObject.FindProperty("actionFunctions");
//                    EditorGUILayout.PropertyField(actionFunctions.GetArrayElementAtIndex(i));
//                    // Apply changes to serialized Objects
//                    serializedObject.ApplyModifiedProperties();
//                    EditorGUILayout.EndVertical();
//                }

//                for (int space = 0; space < 2; space++)
//                    EditorGUILayout.Space();

//                EditorGUILayout.EndHorizontal();

//                EditorGUILayout.EndVertical();
//            }
//        }
//    }
//}