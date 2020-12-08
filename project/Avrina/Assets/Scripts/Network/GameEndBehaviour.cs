using Mirror;
using UnityEngine;

[RequireComponent(typeof(SceneLoader))]
public class GameEndBehaviour : NetworkBehaviour
{
    /// <summary>
    ///  Will store the player name who won
    /// </summary>
    // Ramajana05
    [SyncVar]
    public int wonPlayerId;
    /// <summary>
    ///  Did the first or second player win
    /// </summary>
    [SyncVar]
    public bool wonSecondPlayer;


    /// <summary>
    ///  Will be called on each client after the game ended
    /// </summary>
    public override void OnStartClient()
    {
        this.GetComponent<SceneLoader>().LoadScene(true);

        if (this.wonPlayerId != NetworkClient.connection.connectionId)
        {
            Debug.Log("You lost!");
        }
        else if (this.wonSecondPlayer)
        {
            Debug.Log("Second Player won!");
        }
        else
        {
            Debug.Log("First Player won!");
        }
    }
}
