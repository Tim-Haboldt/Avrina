using UnityEngine;
using System.Collections.Generic;

public class MagicSystemSettings : MonoBehaviour
{
    // Stores the information which keycode is related to the magic key
    private Dictionary<MagicSystemKey, KeyCode> keyMapper;
    // Used by the editor because dictionaries are not serializable
    public List<KeyCollection> keyMapperAsList;
    // Which was the first element pressed
    // If there was no element pressed before - the state will be None
    public MagicSystemElement firstElement;

    // Start is called before the first frame update
    void Start()
    {
        this.firstElement = MagicSystemElement.None;

        this.keyMapper = new Dictionary<MagicSystemKey, KeyCode>();
        foreach (var keyPair in this.keyMapperAsList)
        {
            this.keyMapper.Add(keyPair.magicKey, keyPair.unityKey);
        }
    }

    /**
     * Tests if an specific Magic Key is currently pressed
     */ 
    public bool IsMagicSystemKeyPressed(MagicSystemKey magicKey)
    {
        return Input.GetKeyDown(keyMapper[magicKey]);
    }

    /**
    * Returns if there was an element selected before
    */
    public bool IsFirstElementSelected()
    {
        return (this.firstElement != MagicSystemElement.None);
    }
}
