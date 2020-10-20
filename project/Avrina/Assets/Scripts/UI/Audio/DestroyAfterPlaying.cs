using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class DestroyAfterPlaying : MonoBehaviour
{
    /// <summary>
    ///  Audio source object. Will be used to play the audio
    /// </summary>
    private AudioSource audioSource = null;
    /// <summary>
    ///  Start time of the audio playing
    /// </summary>
    private float startTime = 0f;


    /// <summary>
    ///  Plays the sound from the audio source and destorys the gameobject afterwards
    /// </summary>
    public void Play()
    {
        DontDestroyOnLoad(this.gameObject);

        this.audioSource = GetComponent<AudioSource>();
        this.audioSource.Play();

        this.startTime = Time.time;
    }

    /// <summary>
    ///  Destroys itself after the soundclip finished playing
    /// </summary>
    public void Update()
    {
        if (this.startTime + this.audioSource.clip.length < Time.time)
        {
            Destroy(this.gameObject);
        }
    }
}
