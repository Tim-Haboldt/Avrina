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
    abstract protected Element element { get; }

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
                    case (Element.Fire):
                        this.FireElementWasFirst();
                        break;
                    case (Element.Water):
                        this.WaterElementWasFirst();
                        break;
                    case (Element.Earth):
                        this.EarthElementWasFirst();
                        break;
                    case (Element.Air):
                        this.AirElementWasFirst();
                        break;
                    default:
                        throw new System.NotImplementedException();
                }
                settings.firstElement = Element.None;
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
