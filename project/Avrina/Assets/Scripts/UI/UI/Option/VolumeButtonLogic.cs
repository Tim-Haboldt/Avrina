using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(RectTransform))]
public class VolumeButtonLogic : MonoBehaviour
{
    public enum ButtonState
    {
        MUTE,
        VOLUME,
    }

    public enum ButtonType
    {
        SOUND_EFFECT,
        MUSIC
    }

    /// <summary>
    ///  What is the sprite of the mute button
    /// </summary>
    [SerializeField] private Sprite muteButtonSprite = null;
    /// <summary>
    ///  What is the size of the mute button sprite
    /// </summary>
    [SerializeField] private Vector2 muteButtonSpriteSize = Vector2.zero;
    /// <summary>
    ///  What is the sprite of the volume button
    /// </summary>
    [SerializeField] private Sprite volumeButtonSprite = null;
    /// <summary>
    ///  What is the size of the volume button sprite
    /// </summary>
    [SerializeField] private Vector2 volumeButtonSpriteSize = Vector2.zero;
    /// <summary>
    ///  Stores the type of the button
    /// </summary>
    [SerializeField] private ButtonType buttonType = ButtonType.MUSIC;

    /// <summary>
    ///  What is the current state of the button
    /// </summary>
    public ButtonState state { private set; get; } = ButtonState.VOLUME;
    /// <summary>
    ///  Used to change the image of the 
    /// </summary>
    private Image uiImageElement = null;
    /// <summary>
    ///  Used to set the size of the ui element
    /// </summary>
    private RectTransform uiTransform = null;


    /// <summary>
    ///  Gets the ui image element
    /// </summary>
    private void Start()
    {
        this.uiImageElement = this.GetComponent<Image>();
        this.uiTransform = this.GetComponent<RectTransform>();
    }

    /// <summary>
    ///  Change state of the button
    /// </summary>
    public void MuteUnmuteButton()
    {
        if (this.state == ButtonState.VOLUME)
        {
            this.state = ButtonState.MUTE;

            this.uiImageElement.sprite = this.muteButtonSprite;
            this.uiTransform.sizeDelta = this.muteButtonSpriteSize;

            if (this.buttonType == ButtonType.MUSIC)
            {
                AudioStorage.isMusicMuted = true;
            }
            else
            {
                AudioStorage.areSoundEffectsMuted = true;
            }
        }
        else
        {
            this.state = ButtonState.VOLUME;

            this.uiImageElement.sprite = this.volumeButtonSprite;
            this.uiTransform.sizeDelta = this.volumeButtonSpriteSize;

            if (this.buttonType == ButtonType.MUSIC)
            {
                AudioStorage.isMusicMuted = false;
            }
            else
            {
                AudioStorage.areSoundEffectsMuted = false;
            }
        }
    }
}
