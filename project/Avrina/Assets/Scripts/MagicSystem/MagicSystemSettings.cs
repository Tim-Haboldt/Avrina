using UnityEngine;
using System.Collections.Generic;

public class MagicSystemSettings : MonoBehaviour
{
    // Maps the magicSystemKey to the corresponding Unity Keycode 
    public Dictionary<MagicSystemKey, KeyCode> keyMapper;
    // Which was the first element pressed
    // If there was no element pressed before the state will be NONE
    private MagicSystemElement firstElement;

    // Start is called before the first frame update
    void Start()
    {
        this.firstElement = MagicSystemElement.NONE;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public bool isMagicSystemKeyPressed(MagicSystemKey key)
    {
        return Input.GetKey(keyMapper[key]);
    }
}
