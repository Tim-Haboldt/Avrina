using UnityEngine;
using Mirror;
using UnityEngine.Experimental.Rendering.Universal;

public class PlayerStatus : NetworkBehaviour
{
    public enum InitStatus
    {
        PLAYER_ONE,
        PLAYER_TWO,
        UNKNOWN,
    }

    private enum SoundType
    {
        BURNING,
        FREEZE,
        MELTING
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

    [Header("Status Effect Settings")]
    /// <summary>
    ///  Used to start the frozen and the burning animation
    /// </summary>
    [SerializeField] private Animator effectAnimator;
    /// <summary>
    ///  Stores the instance of the sprite renderer for the effects
    /// </summary>
    [SerializeField] private SpriteRenderer playerRenderer;
    /// <summary>
    ///  Used to stop the animation if the player is frozen
    /// </summary>
    [SerializeField] private Animator playerAnimator;
    /// <summary>
    ///  Will be used to set the ambient throught light
    /// </summary>
    [SerializeField] private Light2D effectStateLight;

    [Header("Status Effect Burning")]
    /// <summary>
    ///  This particle system animation will be played every time the player burns
    /// </summary>
    [SerializeField] private ParticleSystem burnAnimation;
    /// <summary>
    ///  Sets the color of the light if the player is on fire
    /// </summary>
    [SerializeField] private Color onFireLightColor;
    /// <summary>
    ///  Sets the color of the player while he is on fire
    /// </summary>
    [SerializeField] private Color onFirePlayerColor;
    /// <summary>
    ///  Will be played everytime the player takes burn damage
    /// </summary>
    [SerializeField] private AudioClip burnSound;
    /// <summary>
    ///  How load is the burn sound by default
    /// </summary>
    [SerializeField] private float burnSoundVolume;
    /// <summary>
    ///  Used to set the texture in the middle of the player
    /// </summary>
    [SerializeField] private Vector2 burnTextureOffset;

    [Header("Status Effect Frozen")]
    /// <summary>
    ///  This particle system animation will be played every time the player freezes
    /// </summary>
    [SerializeField] private ParticleSystem frozenAnimation;
    /// <summary>
    ///  Will be played only if the player freezes
    /// </summary>
    [SerializeField] private AudioClip freezeSound;
    /// <summary>
    ///  How load is the freeze sound by default
    /// </summary>
    [SerializeField] private float freezeSoundVolume;
    /// <summary>
    ///  Used to set the texture in the middle of the player
    /// </summary>
    [SerializeField] private Vector2 frozenTextureOffset;
    /// <summary>
    ///  Will be played only if the player unfreezes by natural cause
    /// </summary>
    [SerializeField] private AudioClip meltSound;
    /// <summary>
    ///  How load is the melt sound by default
    /// </summary>
    [SerializeField] private float meltSoundVolume;
    /// <summary>
    ///  Used to set the texture in the middle of the player
    /// </summary>
    [SerializeField] private Vector2 meltTextureOffset;
    /// <summary>
    ///  Sets the color of the light if the player is on frozen
    /// </summary>
    [SerializeField] private Color frozenLightColor;
    /// <summary>
    ///  Sets the color of the player if the player is frozen
    /// </summary>
    [SerializeField] private Color frozenPlayerColor;

    [Header("Status Effect Wet")]
    /// <summary>
    ///  This particle system animation will be played while the player is wet
    /// </summary>
    [SerializeField] private ParticleSystem wetAnimation;

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
                var lifeBar = Instantiate(this.lifeBarP1).gameObject.GetComponentInChildren<LifeBar>();
                lifeBar.SetPlayerName(PlayerInformation.playerName);
                return lifeBar;
            case InitStatus.PLAYER_TWO:
                var lifeBar2 = Instantiate(this.lifeBarP2).gameObject.GetComponentInChildren<LifeBar>();
                lifeBar2.SetPlayerName(PlayerInformation.playerName + "Player Two");
                return lifeBar2;
        }

        return null;
    }

    /// <summary>
    ///  Will be called everytime the life points change
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
    [Server]
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
            (NetworkManager.singleton as Server).HandlePlayerDeath(this.connectionToClient, this.GetComponent<PlayerStateManager>().isSecondPlayer);
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
    [Server]
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
                case StatusEffect.COLD:
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
                case StatusEffect.COLD:
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
                    this.RpcPlaySound(SoundType.FREEZE);
                    this.ApplyDamage(this.frozenDamage);
                    break;
                case StatusEffect.NONE:
                    this.statusEffect = StatusEffect.COLD;
                    break;
            }
        }

        if (oldStatusEffect != this.statusEffect)
        {
            this.hasNewStatusEffectSince = Time.time;
            this.RpcUpdateParticleAnimations();
        }

        Debug.Log(this.statusEffect);
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
                    this.timeSinceLastFireTick += this.fireTickRate;

                    this.RpcPlaySound(SoundType.BURNING);
                }
                if (Time.time >= this.hasNewStatusEffectSince + this.howLongIsThePlayerBurning)
                {
                    this.statusEffect = StatusEffect.NONE;
                    this.RpcUpdateParticleAnimations();
                    this.hasNewStatusEffectSince = this.hasNewStatusEffectSince + this.howLongIsThePlayerBurning;
                }
                break;
            case StatusEffect.FROZEN:
                if (Time.time >= this.hasNewStatusEffectSince + this.howLongIsThePlayerFrozen)
                {
                    this.statusEffect = StatusEffect.WET;
                    this.hasNewStatusEffectSince = this.hasNewStatusEffectSince + this.howLongIsThePlayerFrozen;

                    this.RpcUpdateParticleAnimations();
                    this.effectAnimator.SetBool("IsBreaking", true);
                    this.effectAnimator.transform.localPosition = this.meltTextureOffset;
                    this.RpcPlaySound(SoundType.MELTING);
                }
                break;
            case StatusEffect.WET:
                if (Time.time >= hasNewStatusEffectSince + this.howLongIsThePlayerWet)
                {
                    this.statusEffect = StatusEffect.NONE;
                    this.RpcUpdateParticleAnimations();
                    this.hasNewStatusEffectSince = this.hasNewStatusEffectSince + this.howLongIsThePlayerWet;
                }
                break;
        }
    }

    /// <summary>
    ///  Will play the current particle animation
    /// </summary>
    [ClientRpc]
    private void RpcUpdateParticleAnimations()
    {
        this.burnAnimation.Stop();
        this.frozenAnimation.Stop();
        this.wetAnimation.Stop();

        this.effectAnimator.SetBool("IsBreaking", false);

        switch(this.statusEffect)
        {
            case StatusEffect.ON_FIRE:
                this.burnAnimation.Play();
                this.effectAnimator.SetBool("IsBurning", true);
                this.effectAnimator.SetBool("IsFrozen", false);
                this.effectAnimator.transform.localPosition = this.burnTextureOffset;
                this.effectStateLight.color = this.onFireLightColor;
                this.playerRenderer.color = this.onFirePlayerColor;
                this.effectStateLight.enabled = true;
                this.playerAnimator.enabled = true;
                break;
            case StatusEffect.FROZEN:
                this.frozenAnimation.Play();
                this.effectAnimator.SetBool("IsBurning", false);
                this.effectAnimator.SetBool("IsFrozen", true);
                this.effectAnimator.transform.localPosition = this.frozenTextureOffset;
                this.effectStateLight.color = this.frozenLightColor;
                this.playerRenderer.color = this.frozenPlayerColor;
                this.effectStateLight.enabled = true;
                this.playerAnimator.enabled = false;
                break;
            case StatusEffect.WET:
                this.wetAnimation.Play();
                this.effectAnimator.SetBool("IsBurning", false);
                this.effectAnimator.SetBool("IsFrozen", false);
                this.effectStateLight.enabled = false;
                this.playerAnimator.enabled = true;
                this.playerRenderer.color = Color.white;
                break;
            default:
                this.effectAnimator.SetBool("IsBurning", false);
                this.effectAnimator.SetBool("IsFrozen", false);
                this.effectStateLight.enabled = false;
                this.playerAnimator.enabled = true;
                this.playerRenderer.color = Color.white;
                break;
        }
    }

    /// <summary>
    ///  Will be called on every client to play a sound
    /// </summary>
    [ClientRpc]
    private void RpcPlaySound(SoundType soundType)
    {
        AudioClip sound;
        float volume;

        switch (soundType)
        {
            case SoundType.BURNING:
                sound = this.burnSound;
                volume = this.burnSoundVolume;
                break;
            case SoundType.FREEZE:
                sound = this.freezeSound;
                volume = this.freezeSoundVolume;
                break;
            case SoundType.MELTING:
                sound = this.meltSound;
                volume = this.meltSoundVolume;
                break;
            default:
                throw new System.Exception("Sound Type not implemented!");
        }

        if (!AudioStorage.areSoundEffectsMuted)
        {
            AudioSource.PlayClipAtPoint(sound, this.transform.position, AudioStorage.soundEffectVolume * volume);
        }
    }
}
