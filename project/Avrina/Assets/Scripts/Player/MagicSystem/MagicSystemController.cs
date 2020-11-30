using UnityEngine;
using Mirror;

[RequireComponent(typeof(SpellStorage))]
public class MagicSystemController : NetworkBehaviour
{
    /// <summary>
    ///  Reference to the animation handler of the first spirit
    /// </summary>
    [HideInInspector] public SpiritStateManager firstSpirit;
    /// <summary>
    ///  Reference to the animation handler of the second spirit
    /// </summary>
    [HideInInspector] public SpiritStateManager secondSpirit;
    /// <summary>
    ///  Used to get the information about pressed inputs and occoured collisions
    /// </summary>
    public InputController inputController { private get; set; }
    /// <summary>
    ///  Stores all Spellcombinations
    /// </summary>
    private SpellStorage spells;
    /// <summary>
    ///  State of the magic system
    /// </summary>
    private MagicSystemState state;
    /// <summary>
    ///  Stores the cast indicator game object that indicats in which direction the spell is cast
    /// </summary>
    public CastDirection castIndicator;


    /// <summary>
    ///  Defines some default values on the start of the game
    /// </summary>
    void Start()
    {
        this.spells = this.GetComponent<SpellStorage>();
        this.state = MagicSystemState.ElementSelection;
        this.castIndicator.gameObject.SetActive(false);
    }

    /// <summary>
    ///  Handles the state and the inputs for the magic system
    /// </summary>
    private void Update()
    {
        switch (this.state) {
            case MagicSystemState.ElementSelection:
                this.SelectElements();
                break;
            case MagicSystemState.SelectSpellcastDirection:
                this.SelectSpellCastDirection();
                break;
            case MagicSystemState.SpellCasting:
                this.CastSpell();
                break;
        }
        
        // Cancel Spell cast
        if (this.inputController.cancelInput)
        {
            this.firstSpirit.CmdUpdateState(SpiritState.None);
            this.secondSpirit.CmdUpdateState(SpiritState.None);
            this.state = MagicSystemState.ElementSelection;
            this.castIndicator.gameObject.SetActive(false);
        }
    }

    /// <summary>
    ///  Handles the inputs for the element selection
    /// </summary>
    private void SelectElements()
    {
        // Stores the element inputs inside the spirits
        var elementInput = this.GetElementInput();
        if (elementInput != SpiritState.None)
        {
            if (this.firstSpirit.state == SpiritState.None)
            {
                this.firstSpirit.CmdUpdateState(elementInput);
            }
            else if (this.secondSpirit.state == SpiritState.None)
            {
                this.secondSpirit.CmdUpdateState(elementInput);
            }
        }

        // Cast Spell if both spirits have en element selected
        if (this.firstSpirit.state != SpiritState.None && this.secondSpirit.state != SpiritState.None)
        {
            if (!this.inputController.castInput && this.inputController.type != MappingType.JoyStick)
            {
                return;
            }

            this.state = MagicSystemState.SelectSpellcastDirection;
            this.castIndicator.gameObject.SetActive(true);
        }
    }

    /// <summary>
    ///  Shows an arrow to select the spell cast direction
    /// </summary>
    private void SelectSpellCastDirection()
    {
        this.castIndicator.UpdateDirection(this.transform.position, this.inputController.aimDirecton);

        switch (this.inputController.type)
        {
            case MappingType.JoyStick:
                if (this.inputController.fireElementInput || this.inputController.castInput || this.inputController.waterElementInput || this.inputController.castInput)
                {
                    this.state = MagicSystemState.SpellCasting;
                    this.castIndicator.gameObject.SetActive(false);
                }
                break;
            case MappingType.KeyBoard:
            case MappingType.Mouse:
                if (this.inputController.castInput)
                {
                    this.state = MagicSystemState.SpellCasting;
                    this.castIndicator.gameObject.SetActive(false);
                }
                break;
        }
    }

    /// <summary>
    ///  Plays the spellcast animation and creates the spell
    /// </summary>
    private void CastSpell()
    {
        var firstState = this.firstSpirit.state;
        var secondState = this.secondSpirit.state;

        this.firstSpirit.CmdUpdateState(SpiritState.None);
        this.secondSpirit.CmdUpdateState(SpiritState.None);
        this.state = MagicSystemState.ElementSelection;

        this.CmdCreateSpell(firstState, secondState, this.transform.position, this.castIndicator.currentOrientation);
    }

    /// <summary>
    ///  Will create the spell serverside for all players
    /// </summary>
    /// <param name="firstElement">Used to find the spell instance</param>
    /// <param name="secondElement">Used to find the spell instance</param>
    /// <param name="position">Position were the spell will be created</param>
    /// <param name="orientation">Orientation of the spell</param>
    [Command]
    private void CmdCreateSpell(SpiritState firstElement, SpiritState secondElement, Vector2 position, Vector2 orientation)
    {
        var spell = this.spells.GetCopyOfSpell(firstElement, secondElement);
        spell.playerPosition = position;
        spell.castDirection = orientation;
        spell.caster = this.GetComponent<NetworkIdentity>().netId;
        NetworkServer.Spawn(spell.gameObject);
    }

    /// <summary>
    ///  Checks which element input was pressed and returns the corresponding spirit state.
    ///  If no right input was given return state None
    /// </summary>
    /// <returns></returns>
    private SpiritState GetElementInput()
    {
        var elementInput = SpiritState.None;

        if (this.inputController.airElementInput)
        {
            elementInput = SpiritState.Air;
        }
        else if (this.inputController.earthElementInput)
        {
            elementInput = SpiritState.Earth;
        }
        else if (this.inputController.fireElementInput)
        {
            elementInput = SpiritState.Fire;
        }
        else if (this.inputController.waterElementInput)
        {
            elementInput = SpiritState.Water;
        }

        return elementInput;
    }
}
