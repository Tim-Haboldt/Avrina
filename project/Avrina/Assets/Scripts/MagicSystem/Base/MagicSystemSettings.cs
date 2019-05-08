using UnityEngine;
using System.Collections.Generic;

public class MagicSystemSettings : MonoBehaviour
{
    public List<KeyCollection> keyMapper;
    // Which was the first element pressed
    // If there was no element pressed before - the state will be None
    public MagicSystemElement firstElement;

    // Start is called before the first frame update
    void Start()
    {
        this.firstElement = MagicSystemElement.None;
    }

    /**
     * Tests if an specific Magic Key is currently pressed
     */ 
    public bool IsMagicSystemKeyPressed(MagicSystemKey magicKey)
    {
        foreach (var keyCollection in this.keyMapper)
        {
            if (keyCollection.magicKey == magicKey)
            {
                return Input.GetKeyDown(keyCollection.unityKey);
            }
        }

        return false;
    }

    /**
    * Returns if there was an element selected before
    */
    public bool IsFirstElementSelected()
    {
        return (this.firstElement != MagicSystemElement.None);
    }
}
