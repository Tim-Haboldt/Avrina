using UnityEngine;

[System.Serializable]
public struct KeyCollection
{
    public KeyCode unityKey;
    public MagicSystemKey magicKey;

    public KeyCollection(MagicSystemKey magicKey, KeyCode unityKey)
    {
        this.unityKey = unityKey;
        this.magicKey = magicKey;
    }
}
