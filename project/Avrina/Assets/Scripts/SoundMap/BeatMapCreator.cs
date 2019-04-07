using UnityEngine;
using System.Collections.Generic;

public class BeatMapCreator : MonoBehaviour
{
    // tick sound source
    public AudioClip tickSource;
    // tick Sound player
    private AudioSource tickSound;
    // song the map is created for
    public AudioClip trackSource;
    // song as playable element
    private AudioSource track;
    // bpm of the song the map is created for
    public float trackBPM;
    // how long does it take until the first beat of the song is played
    public float offset;
    // how many ticks are per beat
    // defines how many part per beat can be marked as important.
    // (how big is the data of the map)
    public int markableTicksPerBeat;
    // used to display the current map
    public Canvas beatMapUI;
    // stores the information how many posible beats can be marked as important
    // and which of them are important
    private List<bool> trackTicks;
    // where is the current position in the song
    // (what part of the song need to be played next and what is the next tick)
    private int currentTick;
    // how many tick per second
    private float ticksPerSecond;

    private void Start()
    {
        this.setupSoundAndMap();
        //this.beatMapUI.add Panel
    }

    private void Update()
    {
        this.handleKeyInputs();
    }

    private void handleKeyInputs()
    {
        // this key starts or stops the track
        if (Input.GetKeyDown(KeyCode.Space))
        {

        }
        // this key goes a tick back in the song
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {

        }
        // this key skips a tick
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {

        }
    }
    
    /**
     * creates an List which contains all posible ticks for given song
     */
    private List<bool> createTrackTicksList(float trackBPM, float clipLength, int markableTicksPerBeat, out float ticksPerSecond)
    {
        // create instance of map
        List<bool> trackTicks = new List<bool>();

        // calculate the amout of ticks in the track
        float bpmPerSecond = trackBPM / 60;
        float numberOfBeats = clipLength * bpmPerSecond;
        int numberOfTicks = Mathf.RoundToInt(numberOfBeats * markableTicksPerBeat);

        // stores the amout of ticks per second
        ticksPerSecond = bpmPerSecond * markableTicksPerBeat;

        // create the map size corresponding to the amout of ticks in the track
        for (int i = 0; i < numberOfTicks; i++)
        {
            trackTicks.Add(false);
        }

        // returns created map
        return trackTicks;
    }

    /**
     * loads all sound files
     * creates the map
     */ 
    private void setupSoundAndMap()
    {
        // load tick sound into the script
        this.tickSound = this.gameObject.AddComponent<AudioSource>();
        this.tickSound.playOnAwake = false;
        this.tickSound.clip = this.tickSource;
        // load track into the script
        this.track = this.gameObject.AddComponent<AudioSource>();
        this.track.playOnAwake = false;
        this.track.clip = this.trackSource;
        // create tick map for the given song
        this.trackTicks = createTrackTicksList(this.trackBPM, this.track.clip.length, this.markableTicksPerBeat, out this.ticksPerSecond);
        // set current play position to 0
        this.currentTick = 0;
    }
}
