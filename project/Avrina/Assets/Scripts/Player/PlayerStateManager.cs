//using UnityEngine;
//using System.Collections.Generic;
//using UnityEngine.Events;
//using System;
//using System.Linq;

//[RequireComponent(typeof(PlayerInputController))]
//public class PlayerStateManager : MonoBehaviour
//{
//    // Current player state
//    [HideInInspector] public PlayerState playerState;
//    // Stores what exit condition each state has (you can add multiple per state)
//    [HideInInspector] public Dictionary<PlayerState, List<PlayerMovement>> stateExitConditions;
//    // Stores which actions will be performed for each state
//    [HideInInspector] public Dictionary<PlayerState, List<PlayerMovement>> stateActions;
//    // Contains all Inputs from the player
//   // [HideInInspector] private PlayerInputController inputs;

//    // Inspector Variables
//    // We need a extra list because unity cannot save dicionaries
//    [HideInInspector] public List<List<PlayerMovement>> exitConditions;
//    [HideInInspector] public List<List<PlayerMovement>> actionFunctions;
//    // Because enums can change we need a clever way to display them in the editor
//    [HideInInspector] public List<PlayerState> selectedPlayerState;

//    private void Start()
//    {
//        //this.inputs = this.GetComponent<PlayerInputController>();

//        // Transform lists into dictionary
//        this.stateActions = new Dictionary<PlayerState, List<PlayerMovement>>();
//        this.stateExitConditions = new Dictionary<PlayerState, List<PlayerMovement>>();
//        for (int i = 0; i < this.selectedPlayerState.Count; i++)
//        {
//            var selectedState = this.selectedPlayerState[i];
//            this.stateActions.Add(selectedState, this.actionFunctions[i]);
//            this.stateExitConditions.Add(selectedState, this.exitConditions[i]);
//        }
//    }

//    private void Update()
//    {
//        /*// Update current State corresponding to the given inputs
//        foreach (this.stateExitConditions[this.playerState].Invoke(this.inputs);
//        // Run actions corresponding to the current player state
//        var stateInformation = new StateInformation();
//        foreach (PlayerMovement movement in this.stateActions[this.playerState]) {
//            stateInformation = movement.calculateNextState(this.inputs, stateInformation);
//        }*/
//    }
//}
