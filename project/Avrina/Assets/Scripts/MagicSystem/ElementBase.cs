using UnityEngine;

[RequireComponent(typeof(MagicSystemSettings))]
public class ElementBase : MonoBehaviour
{
    // Stores keybindings and other settings
    private MagicSystemSettings settings;
    // Element key and direction
    [SerializeField] public MagicSystemKey key;

    // Start is called before the first frame update
    void Start()
    {
        this.settings = GetComponent<MagicSystemSettings>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
