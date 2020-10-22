using UnityEngine;
using Mirror;

public class PlayerStatus : NetworkBehaviour
{
    public enum InitStatus
    {
        PLAYER_ONE,
        PLAYER_TWO,
        UNKNOWN,
    }

    /// <summary>
    ///  Current status effect of the player
    /// </summary>
    [SyncVar]
    public StatusEffect statusEffect = StatusEffect.DEFAULT;
    /// <summary>
    ///  Current health of the player
    /// </summary>
    [SyncVar(hook = nameof(LifePointsChanged))]
    public float lifePoints = 1f;
    /// <summary>
    ///  Life bar ui object for player one
    /// </summary>
    [SerializeField] private Canvas lifeBarP1;
    /// <summary>
    ///  Life bar ui object for player two
    /// </summary>
    [SerializeField] private Canvas lifeBarP2;

    /// <summary>
    ///  Stores the inputController instance
    /// </summary>
    [HideInInspector] public InputController inputController = null;
    /// <summary>
    ///  Life Bar UI of the player
    /// </summary>
    private LifeBar lifeBar = null;
    /// <summary>
    ///  Stores the current init status of the player status
    /// </summary>
    private InitStatus initStatus = InitStatus.UNKNOWN;

    /// <summary>
    ///  Finds the current life bar object and stores it inside the player status
    /// </summary>
    public void Init(InitStatus initStatus)
    {
        if (initStatus == InitStatus.UNKNOWN)
        {
            return;
        }

        this.lifeBar = this.InstantiateLifeBarForPlayer(initStatus);
        this.lifeBar.gameObject.SetActive(true);
    }

    /// <summary>
    ///  Instanciates and returns the lifebar object for the corresponding player
    /// </summary>
    /// <returns></returns>
    private LifeBar InstantiateLifeBarForPlayer(InitStatus initStatus)
    {
        switch (initStatus)
        {
            case InitStatus.PLAYER_ONE:
                return Instantiate(this.lifeBarP1).gameObject.GetComponentInChildren<LifeBar>();
            case InitStatus.PLAYER_TWO:
                return Instantiate(this.lifeBarP2).gameObject.GetComponentInChildren<LifeBar>();
        }

        Debug.LogError("Invalid InitState for PlayerStatus! InitStatus: " + initStatus);
        return null;
    }

    /// <summary>
    ///  
    /// </summary>
    /// <param name="oldLifePoints"></param>
    /// <param name="newLifePoints"></param>
    private void LifePointsChanged(float oldLifePoints, float newLifePoints)
    {
        if (this.lifeBar == null)
        {
            return;
        }

        this.lifeBar.UpdateLifeBar(newLifePoints);
    }
}
