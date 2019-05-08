using UnityEngine;

[RequireComponent(typeof(MagicSystemSettings))]
public abstract class ElementBase : MonoBehaviour
{
    // Stores keybindings and other settings
    private MagicSystemSettings settings;
    // Element key and direction
    [SerializeField] public MagicSystemKey key;
    // Every Element needs to know which element they are
    // The variable needs to be overriten in the child class
    abstract protected MagicSystemElement element { get; }

    // Start is called before the first frame update
    void Start()
    {
        this.settings = GetComponent<MagicSystemSettings>();
    }

    // Update is called once per frame
    void Update()
    {
        if (settings.IsMagicSystemKeyPressed(this.key))
        {
            if (settings.IsFirstElementSelected())
            {
                switch (settings.firstElement)
                {
                    case (MagicSystemElement.Fire):
                        this.FireElementWasFirst();
                        break;
                    case (MagicSystemElement.Water):
                        this.WaterElementWasFirst();
                        break;
                    case (MagicSystemElement.Earth):
                        this.EarthElementWasFirst();
                        break;
                    case (MagicSystemElement.Air):
                        this.AirElementWasFirst();
                        break;
                    default:
                        throw new System.NotImplementedException();
                }
                settings.firstElement = MagicSystemElement.None;
            } else
            {
                settings.firstElement = this.element;
            }
        }
    }

    // These function will be called if the second element was selected
    protected abstract void AirElementWasFirst();
    protected abstract void WaterElementWasFirst();
    protected abstract void FireElementWasFirst();
    protected abstract void EarthElementWasFirst();
}
