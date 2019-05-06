using UnityEngine;
using System.Collections.Generic;

public class MagicSystemSettings : MonoBehaviour
{
    // Maps the magicSystemKey to the corresponding Unity Keycode 
    public Dictionary<MagicSystemKey, KeyCode> keyMapper;

    // Start is called before the first frame update
    void Start()
    {
        
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
