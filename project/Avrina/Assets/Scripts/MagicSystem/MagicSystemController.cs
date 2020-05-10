using UnityEngine;

[RequireComponent(typeof(InputController))]
[RequireComponent(typeof(SpellStorage))]
public class MagicSystemController : MonoBehaviour
{
    /// <summary>
    ///  Reference to the animation handler of the first spirit
    /// </summary>
    [SerializeField] private SpiritAnimationHandler firstSpirit;
    /// <summary>
    ///  Reference to the animation handler of the second spirit
    /// </summary>
    [SerializeField] private SpiritAnimationHandler secondSpirit;
    /// <summary>
    ///  Used to get the information about pressed inputs and occoured collisions
    /// </summary>
    private InputController inputController;
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
        this.inputController = this.GetComponent<InputController>();
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
            this.firstSpirit.UpdateState(SpiritState.None);
            this.secondSpirit.UpdateState(SpiritState.None);
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
                this.firstSpirit.UpdateState(elementInput);
            }
            else if (this.secondSpirit.state == SpiritState.None)
            {
                this.secondSpirit.UpdateState(elementInput);
            }
        }

        // Cast Spell if both spirits have en element selected
        if (this.inputController.castInput)
        {
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
                if (this.inputController.fireElementInput || this.inputController.castInput || this.inputController.waterElementInput)
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
