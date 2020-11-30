using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicPlayer : MonoBehaviour
{
    /// <summary>
    ///  Stores the background music for the current scene
    /// </summary>
    [SerializeField] private AudioClip backgroundMusic = null;
    /// <summary>
    ///  Used to make all background music of the same volume
    /// </summary>
    [SerializeField] private float backgroundMusicVolumeFactor = 1f;

    /// <summary>
    ///  Stores the current instance of the music player
    /// </summary>
    public static MusicPlayer musicPlayerInstance = null;
    /// <summary>
    ///  Will be used to play the audio clip
    /// </summary>
    private AudioSource audioSource = null;

    
    /// <summary>
    ///  Will be called on creation of the music player
    /// </summary>
    private void Start()
    {
        if (musicPlayerInstance == null)
        {
            musicPlayerInstance = this;
            musicPlayerInstance.InitMusicPlayer();

            DontDestroyOnLoad(this);
        }
        else
        {
            musicPlayerInstance.PlayNextTrack(this.backgroundMusic, this.backgroundMusicVolumeFactor);

            Destroy(this.gameObject);
        }
    }

    /// <summary>
    ///  Will be called when the music player is created
    /// </summary>
    private void InitMusicPlayer()
    {
        this.audioSource = this.GetComponent<AudioSource>();

        this.audioSource.clip = this.backgroundMusic;
        this.audioSource.loop = true;
        this.UpdateAudio();
        this.audioSource.Play();
    }
    
    /// <summary>
    ///  Will be called 
    /// </summary>
    /// <param name="backgroundMusic"></param>
    private void PlayNextTrack(AudioClip backgroundMusic, float volumeFactor)
    {
        if (this.audioSource.clip == backgroundMusic)
        {
            return;
        }

        this.audioSource.Stop();

        this.backgroundMusicVolumeFactor = volumeFactor;
        this.audioSource.clip = backgroundMusic;
        this.audioSource.loop = true;
        this.UpdateAudio();
        this.audioSource.Play();
    }

    /// <summary>
    ///  Will be called after the music volume changed
    /// </summary>
    public void UpdateAudio()
    {
        if (AudioStorage.isMusicMuted)
        {
            this.audioSource.volume = 0;
        }
        else
        {
            this.audioSource.volume = AudioStorage.musicVolume * this.backgroundMusicVolumeFactor;
        }
    }
}
