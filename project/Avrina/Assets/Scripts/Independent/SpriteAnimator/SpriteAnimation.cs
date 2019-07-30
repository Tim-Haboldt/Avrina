using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class SpriteAnimation
{
    public string name;
    public int fps;
    public Sprite[] frames;
    public UnityEvent exitEvent;
}
