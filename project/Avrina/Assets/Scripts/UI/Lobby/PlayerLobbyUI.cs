using UnityEngine;
using System.Collections.Generic;

public class PlayerLobbyUI : MonoBehaviour
{
    [SerializeField] private PlayerLobbyUIElement playerUIElementOne;
    [SerializeField] private PlayerLobbyUIElement playerUIElementTwo;
    [SerializeField] private PlayerLobbyUIElement playerUIElementThree;
    [SerializeField] private PlayerLobbyUIElement playerUIElementFour;
    [SerializeField] private PlayerLobbyUIElement playerUIElementFive;
    [SerializeField] private PlayerLobbyUIElement playerUIElementSix;
    [SerializeField] private PlayerLobbyUIElement playerUIElementSeven;
    [SerializeField] private PlayerLobbyUIElement playerUIElementEight;

    /// <summary>
    ///  Stores all references to the player UI elements 
    /// </summary>
    public List<PlayerLobbyUIElement> playerUIElements { get; } = new List<PlayerLobbyUIElement>();


    /// <summary>
    ///  Will be called at the start of the game and stores all references to the playerUIElements
    /// </summary>
    private void Awake()
    {
        this.playerUIElements.Add(this.playerUIElementOne);
        this.playerUIElements.Add(this.playerUIElementTwo);
        this.playerUIElements.Add(this.playerUIElementThree);
        this.playerUIElements.Add(this.playerUIElementFour);
        this.playerUIElements.Add(this.playerUIElementFive);
        this.playerUIElements.Add(this.playerUIElementSix);
        this.playerUIElements.Add(this.playerUIElementSeven);
        this.playerUIElements.Add(this.playerUIElementEight);
    }
}
