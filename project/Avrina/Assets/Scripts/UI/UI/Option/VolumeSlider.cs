using UnityEngine;

public class VolumeSlider : MonoBehaviour
{
    public enum SliderType
    {
        SOUND_EFFECT,
        MUSIC
    }

    /// <summary>
    ///  What is the type of the slider
    /// </summary>
    [SerializeField] private SliderType type = SliderType.MUSIC;

    /// <summary>
    ///  Updates the audio settings corresponding to the volume slider
    /// </summary>
    public void OnSliderValueChange(float newValue)
    {
        switch (this.type)
        {
            case SliderType.MUSIC:
                AudioStorage.musicVolume = newValue;
                break;
            case SliderType.SOUND_EFFECT:
                AudioStorage.soundEffectVolume = newValue;
                break;
        }
    }
}
