using UnityEngine;
using System.Collections.Generic;

public class MagicSystemSettings : MonoBehaviour
{
    public List<KeyCollection> keyMapper;
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

    public bool isMagicSystemKeyPressed(MagicSystemKey magicKey)
    {
        foreach (var keyCollection in this.keyMapper)
        {
            if (keyCollection.magicKey == magicKey)
            {
                return Input.GetKey(keyCollection.unityKey);
            }
        }

        return false;
    }
}
