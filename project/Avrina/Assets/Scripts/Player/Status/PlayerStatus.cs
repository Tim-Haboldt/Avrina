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

    [Header("Status Effects Settings")]
    /// <summary>
    ///  Current status effect of the player
    /// </summary>
    [SyncVar]
    public StatusEffect statusEffect = StatusEffect.NONE;
    /// <summary>
    ///  How much more damage will the player dealt if the same status effect is already applied
    /// </summary>
    [SerializeField] private float damageMulitplierForEffectiveStatusEffects;
    /// <summary>
    ///  How much less damage will the player dealt if the opposing status effect is applied
    /// </summary>
    [SerializeField] private float damageMultiplierForIneffectiveStatusEffects;
    /// <summary>
    ///  How much damage will be dealt to the player if he is going to be frozen
    /// </summary>
    [SerializeField] private float frozenDamage;
    /// <summary>
    ///  How long will the player stay frozen
    /// </summary>
    [SerializeField] private float howLongIsThePlayerFrozen;
    /// <summary>
    ///  How long will the player stay wet
    /// </summary>
    [SerializeField] private float howLongIsThePlayerWet;
    /// <summary>
    ///  Defines how much damage the player will be dealt for each fire tick
    /// </summary>
    [SerializeField] private float fireDamage;
    /// <summary>
    ///  After how many seconds will the player take fire damage again
    /// </summary>
    [SerializeField] private float fireTickRate;
    /// <summary>
    ///  How long is the player burning
    /// </summary>
    [SerializeField] private float howLongIsThePlayerBurning;

    [Header("Status Effect Animations")]
    /// <summary>
    ///  This particle system animation will be played every time the player burns
    /// </summary>
    [SerializeField] private ParticleSystem burnAnimation;

    [Header("Life Settings")]
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
    ///  When did the status effect of the player change
    /// </summary>
    private float hasNewStatusEffectSince;
    /// <summary>
    ///  When did the last fire tick occour
    /// </summary>
    private float timeSinceLastFireTick;

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

    /// <summary>
    ///  Will be called every time the player was hit by something
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="statusEffect"></param>
    [Command]
    public void CmdHandleHit(float damage, StatusEffect statusEffect)
    {
        this.ApplyDamage(damage, statusEffect);
        this.UpdateStatusEffect(statusEffect);
    }

    /// <summary>
    ///  Will be called calcualte and apply the damage dealt to the player
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="statusEffect"></param>
    private void ApplyDamage(float damage, StatusEffect statusEffect = StatusEffect.NONE)
    {
        var appliedDamage = damage;

        if (statusEffect == StatusEffect.ON_FIRE && this.statusEffect == StatusEffect.ON_FIRE
            || statusEffect == StatusEffect.FROZEN && this.statusEffect == StatusEffect.WET)
        {
            appliedDamage *= this.damageMulitplierForEffectiveStatusEffects;
        }
        else if (statusEffect == StatusEffect.WET && this.statusEffect == StatusEffect.ON_FIRE
            || statusEffect == StatusEffect.ON_FIRE && this.statusEffect == StatusEffect.WET)
        {
            appliedDamage *= this.damageMultiplierForIneffectiveStatusEffects;
        }

        if (this.lifePoints - appliedDamage <= 0)
        {
            this.lifePoints = 0;
        }
        else
        {
            this.lifePoints -= appliedDamage;
        }
    }

    /// <summary>
    ///  Changes the current status effect corresponding to the new applied one
    /// </summary>
    /// <param name="statusEffect"></param>
    private void UpdateStatusEffect(StatusEffect statusEffect)
    {
        if (statusEffect == StatusEffect.NONE)
        {
            return;
        }

        var oldStatusEffect = this.statusEffect;
        if (statusEffect == StatusEffect.ON_FIRE)
        {
            switch (this.statusEffect)
            {
                case StatusEffect.FROZEN:
                    this.statusEffect = StatusEffect.WET;
                    break;
                case StatusEffect.WET:
                    this.statusEffect = StatusEffect.NONE;
                    break;
                case StatusEffect.NONE:
                    this.statusEffect = StatusEffect.ON_FIRE;
                    this.timeSinceLastFireTick = Time.time;
                    break;
                case StatusEffect.ON_FIRE:
                    this.hasNewStatusEffectSince = Time.time;
                    break;
            }
        }
        else if (statusEffect == StatusEffect.WET)
        {
            switch (this.statusEffect)
            {
                case StatusEffect.ON_FIRE:
                    this.statusEffect = StatusEffect.NONE;
                    break;
                case StatusEffect.NONE:
                    this.statusEffect = StatusEffect.WET;
                    break;
            }
        }
        else if (statusEffect == StatusEffect.FROZEN)
        {
            switch (this.statusEffect)
            {
                case StatusEffect.WET:
                    this.statusEffect = StatusEffect.FROZEN;
                    this.ApplyDamage(this.frozenDamage);
                    break;
            }
        }

        if (oldStatusEffect != this.statusEffect)
        {
            this.hasNewStatusEffectSince = Time.time;
            this.UpdateParticleAnimations();
        }
    }

    /// <summary>
    ///  Will update the player if he's no longer frozen or burning
    /// </summary>
    [Server]
    private void Update()
    {
        switch (this.statusEffect)
        {
            case StatusEffect.ON_FIRE:
                if (Time.time >= this.timeSinceLastFireTick + this.fireTickRate)
                {
                    this.ApplyDamage(this.fireDamage);
                    Debug.Log("On Fire");
                    this.timeSinceLastFireTick += this.fireTickRate;
                }
                if (Time.time >= this.hasNewStatusEffectSince + this.howLongIsThePlayerBurning)
                {
                    this.statusEffect = StatusEffect.NONE;
                    this.UpdateParticleAnimations();
                    this.hasNewStatusEffectSince = this.hasNewStatusEffectSince + this.howLongIsThePlayerBurning;
                }
                break;
            case StatusEffect.FROZEN:
                if (Time.time >= this.hasNewStatusEffectSince + this.howLongIsThePlayerFrozen)
                {
                    this.statusEffect = StatusEffect.WET;
                    this.UpdateParticleAnimations();
                    this.hasNewStatusEffectSince = this.hasNewStatusEffectSince + this.howLongIsThePlayerFrozen;
                }
                break;
            case StatusEffect.WET:
                if (Time.time >= hasNewStatusEffectSince + this.howLongIsThePlayerWet)
                {
                    this.statusEffect = StatusEffect.NONE;
                    this.UpdateParticleAnimations();
                    this.hasNewStatusEffectSince = this.hasNewStatusEffectSince + this.howLongIsThePlayerWet;
                }
                break;
        }
    }

    /// <summary>
    ///  Will play the current particle animation
    /// </summary>
    [Client]
    private void UpdateParticleAnimations()
    {
        this.burnAnimation.Stop();

        switch(this.statusEffect)
        {
            case StatusEffect.ON_FIRE:
                this.burnAnimation.Play();
                break;
        }
    }
}
