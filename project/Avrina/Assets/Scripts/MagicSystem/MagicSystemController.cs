using UnityEngine;

[RequireComponent(typeof(InputController))]
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
    ///  Defines some default values on the start of the game
    /// </summary>
    void Start()
    {
        this.inputController = this.GetComponent<InputController>();
    }

    /// <summary>
    ///  Handles the state and the inputs for the magic system
    /// </summary>
    void Update()
    {
        // Stores the element inputs inside the spirits
        var elementInput = this.GetElementInput();
        if (elementInput != SpiritState.None)
        {
            if (this.firstSpirit.state == SpiritState.None)
            {
                this.firstSpirit.UpdateState(elementInput);
            }
            else if(this.secondSpirit.state == SpiritState.None)
            {
                this.secondSpirit.UpdateState(elementInput);
            }
        }

        // Cancel Spell cast
        if (this.inputController.cancelInput)
        {
            this.firstSpirit.UpdateState(SpiritState.None);
            this.secondSpirit.UpdateState(SpiritState.None);
        }

        // Cast Spell if both spirits have en element selected
        if (this.inputController.castInput)
        {

        }
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
