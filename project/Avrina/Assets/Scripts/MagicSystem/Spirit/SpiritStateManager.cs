using UnityEngine;
using Mirror;

public class SpiritStateManager : NetworkBehaviour
{
    [Header("UI")]
    /// <summary>
    ///  The spirit needs to know which one is the white spirit
    /// </summary>
    [SerializeField] public bool isWhiteSpirit = true;
    /// <summary>
    ///  Does the spirit follow the second player of the client
    /// </summary>
    [SerializeField] public bool followSecondPlayer = false;

    /// <summary>
    ///  Stores the current state of the spirit
    /// </summary>
    [SyncVar(hook = nameof(HandleStateChanged))]
    public SpiritState state = SpiritState.None;
    public void HandleStateChanged(SpiritState oldState, SpiritState newState) => spiritAnimationHandler.UpdateSpiritAppearance(this.state);
    /// <summary>
    ///  Stores a reference of the spirit animation handler
    /// </summary>
    private SpiritAnimationHandler spiritAnimationHandler;
    /// <summary>
    ///  Stores a reference of the spirit follow manager
    /// </summary>
    private SpiritPlayerFollower spiritFollowManager;


    /// <summary>
    ///  Searches for its player object
    ///  and references itself inside the magic system controller
    /// </summary>
    public override void OnStartClient()
    {
        this.spiritAnimationHandler = this.GetComponent<SpiritAnimationHandler>();

        if (!this.hasAuthority)
        {
            return;
        }

        var player = this.GetPlayer();
        this.spiritFollowManager = this.GetComponent<SpiritPlayerFollower>();
        this.spiritFollowManager.Init(this, player);

        var magicSystemController = player.GetComponent<MagicSystemController>();
        if (this.isWhiteSpirit)
        {
            magicSystemController.firstSpirit = this;
        }
        else
        {
            magicSystemController.secondSpirit = this;
        }
    }

    /// <summary>
    ///  Returns an instance of the player object
    /// </summary>
    /// <returns></returns>
    private PlayerStateManager GetPlayer()
    {
        foreach (var player in FindObjectsOfType<PlayerStateManager>())
        {
            if (!player.hasAuthority)
            {
                continue;
            }

            if (this.followSecondPlayer)
            {
                if (player.isLocalPlayer)
                {
                    continue;
                }
            }
            else
            {
                if (!player.isLocalPlayer)
                {
                    continue;
                }
            }

            return player;
        }

        Debug.Log("No Player object found");
        return null;
    }

    /// <summary>
    ///  
    /// </summary>
    /// <param name="nextState"></param>
    [Command]
    public void CmdUpdateState(SpiritState nextState)
    {
        this.state = nextState;
    }
}
