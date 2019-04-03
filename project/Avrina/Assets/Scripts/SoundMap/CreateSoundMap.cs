using UnityEngine;
using System;
using System.Collections.Generic;

public class CreateSoundMap : MonoBehaviour
{
    // Music clip the map is created for
    private AudioSource music;
    // Sound will be played for each tick
    private AudioSource sound;

    // stores the information of the start time
    // the map times are relativ to the start time
    private DateTime startTime;
    private DateTime timeSinceLastTimeStamp;
    private List<DateTime> mapTimes;
    private bool isPlaying;

    void Start()
    {
        this.setup();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            this.mapTimes.Add(DateTime.Now);
            Debug.Log("Time added!");
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            this.music.Stop();
            Debug.Log("Stop!");
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            this.isPlaying = true;
            this.timeSinceLastTimeStamp = this.startTime;
            Debug.Log("Map plays!");
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (this.music.isPlaying)
            {
                this.music.Stop();
            }
            this.setup();
            Debug.Log("Reload Complete!");
        }

        if (this.isPlaying)
        {
            this.playMap();
        }
    }

    private void setup()
    {
        this.isPlaying = false;
        this.mapTimes = new List<DateTime>();
        this.startTime = DateTime.Now;
        this.music.Play();
    }

    private void playMap()
    {
        this.timeSinceLastTimeStamp = this.timeSinceLastTimeStamp.AddSeconds(Time.deltaTime);

        while (this.timeSinceLastTimeStamp > this.mapTimes[0])
        {
            Debug.Log("[" + this.mapTimes.Count + "]: " + (this.mapTimes[0] - this.startTime));
            this.mapTimes.Remove(this.mapTimes[0]);
            if (this.mapTimes.Count <= 0)
            {
                Debug.Log("Playing Finished!");
                this.isPlaying = false;
                return;
            }
        }
    }
}
