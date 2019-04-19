using UnityEngine;
using System.Collections.Generic;

public class BeatMapCreator : MonoBehaviour
{
    #region VARIABLES

    #region SOUND

    // Tick sound clip (actual mp3 file)
    public AudioClip tickClip;
    // Tick sound as playable element
    private AudioSource tickSource;
    // Track clip (actual mp3 file)
    public AudioClip trackTick;
    // Track as playable element
    public AudioSource trackSource;

    #endregion SOUND

    #region MAPSPECIFICATIONS

    // Bpm of the track the map is created for
    public float trackBPM;
    // How long does it take until the first beat of the song is played
    public float offset;
    // How many ticks are per beat
    // Defines how many part per beat can be marked as important.
    public int markableTicksPerBeat;

    #endregion MAPSPECIFICATIONS

    #region UISPECIFICATIONS

    // It defines how much of the map is currently visible
    public int visibleBeats;
    // Defines how big the beat panel in reference to the display size is (ratio)
    public Vector2 beatSize;
    // Defines how big the tick panel in reference to the display size is (ratio)
    public Vector2 tickSize;

    #endregion UISPECIFICATIONS

    #region UPDATE

    // Defines how much time passed since the button was stared pressed down
    private float timeSinceLeftPressStarted;
    // Defines how much time passed since the button was stared pressed down
    private float timeSinceRightPressStarted;
    // Time since the last rythem unit
    private float timeSinceLastRythemUnit;
    // Stores how long it takes until the next rythem unit has to be played
    private float rythmUnitFrequency;

    #endregion UPDATE

    #region PLAYSONG

    // Used to get the offset of the song right for the tick sounds
    private float tmpOffset;
    // Because we need to current ticks we need to replace one
    private int tmpCurrentTick;

    #endregion PLAYSONG

    #region MAPDATA

    // Name of the file the track data is stored into
    public string saveName;
    // Name of the file the track data is loaded from
    public string loadName;

    #endregion MAPDATA

    #region MAPINFO

    // Stores the information how many posible rythm units can be marked as important
    // and which of them are important
    public bool[] trackRythmUnits;
    // Where is the current position in the track
    // (What part of the track need to be played next and what is the next tick)
    public int currentRythmUnit;

    #endregion MAPINFO

    #region UI

    // Contains all UI elements and handels their movement over the screen
    public BeatMapUI mapUI;

    #endregion UI

    #endregion VARIABLES

    #region METHODS

    #region SETUP

    /**
     * This function is the first thing called by the Unity Engine
     * Instance the MapUI as early as possible
     */
    private void OnEnable()
    {
        this.mapUI = new BeatMapUI(this);
    }

    /**
     * Default Unity start method
     */
    private void Start()
    {
        // define some variables
        this.timeSinceLeftPressStarted = 0f;
        this.timeSinceRightPressStarted = 0f;

        // load the map sound and the tick sound
        this.SetupSound();
        // create tick map for the given song
        this.CreateTrackTicksList();

        // setup the UI
        this.mapUI.SetupUI(this.visibleBeats, this.markableTicksPerBeat, this.beatSize, this.tickSize);
        this.mapUI.UpdateUI(this.trackRythmUnits, this.currentRythmUnit);
    }

    /**
     * Loads all sound files
     */
    private void SetupSound()
    {
        // Create container for the audio player (sources)
        var audioContainer = new GameObject("AudioContainer");
        audioContainer.transform.SetParent(this.transform, false);
        // Load tick sound into the script
        this.tickSource = audioContainer.AddComponent<AudioSource>();
        this.tickSource.playOnAwake = false;
        this.tickSource.clip = this.tickClip;
        // Load track into the script
        this.trackSource = audioContainer.AddComponent<AudioSource>();
        this.trackSource.playOnAwake = false;
        this.trackSource.clip = this.trackTick;
        // Set current play position to 0
        this.currentRythmUnit = 0;
    }

    /**
     * Creates an List which contains all possible rythm units for given song
     */
    public void CreateTrackTicksList()
    {
        // Reset current rythm unit pointer
        this.currentRythmUnit = 0;
        // Calculate the amout of ticks and beats in the track
        var beatsPerSecond = this.trackBPM / 60;
        var numberOfBeats = Mathf.RoundToInt(this.trackSource.clip.length * beatsPerSecond);
        var numberOfTicks = numberOfBeats * this.markableTicksPerBeat;

        // Stores the amout of rythem units per second (beats and ticks per second)
        var ticksPerSecond = beatsPerSecond * this.markableTicksPerBeat;
        var rythemUnitsPerSecond = beatsPerSecond + ticksPerSecond;
        this.rythmUnitFrequency = 1 / rythemUnitsPerSecond;

        // Create instance for the rythem unity array
        // There is no need to set all values to false, because the default value is false
        this.trackRythmUnits = new bool[numberOfBeats + numberOfTicks];
    }
            
    #endregion SETUP

    #region UPDATE

    /**
     *Ddefault Unity update method
     */
    private void Update()
    {
        this.HandleKeyInputs();

        if (this.trackSource.isPlaying)
        {
            if (this.tmpOffset != 0)
            {
                this.tmpOffset -= Time.deltaTime;

            }
            if (this.tmpOffset > 0)
                return;

            if (this.tmpOffset != 0)
            {
                this.timeSinceLastRythemUnit += this.tmpOffset;
                this.tmpOffset = 0;
                return;
            }

            this.timeSinceLastRythemUnit += Time.deltaTime;
            if (this.timeSinceLastRythemUnit >= this.rythmUnitFrequency)
            {
                this.timeSinceLastRythemUnit -= this.rythmUnitFrequency;
                this.tmpCurrentTick++;
                if (this.trackRythmUnits[this.tmpCurrentTick])
                {
                    this.tickSource.Play();
                }
            }
        }
    }

    /**
     * Handles all key inputs
     */ 
    private void HandleKeyInputs()
    {
        // Saves the map as json
        if (Input.GetKeyDown(KeyCode.Q))
        {
            this.SaveMap();
        }

        // This key starts or stops the track
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!this.trackSource.isPlaying)
            {
                this.trackSource.Play();
                this.timeSinceLastRythemUnit = 0;
                this.tmpCurrentTick = 0;
                this.tmpOffset = this.offset;
            } else
            {
                this.trackSource.Stop();
            }
        }

        // This key goes a tick back in the song
        if (Input.GetKeyDown(KeyCode.A))
        {
            this.StepToTheLeft();
        }
        // This key goes a tick forward
        if (Input.GetKeyDown(KeyCode.D))
        {
            this.StepToTheRight();
        }
        // If the right key is pressed long enough the map will step fast to the right
        if (Input.GetKey(KeyCode.A))
        {
            this.timeSinceLeftPressStarted += Time.deltaTime;
            if (this.timeSinceLeftPressStarted > 0.4f)
            {
                this.StepToTheLeft();
            }
        }
        else
        {
            this.timeSinceLeftPressStarted = 0f;
        }
        // If the left key is pressed long enough the map will step fast to the left
        if (Input.GetKey(KeyCode.D))
        {
            this.timeSinceRightPressStarted += Time.deltaTime;
            if (this.timeSinceRightPressStarted > 0.4f)
            {
                this.StepToTheRight();
            }
        }
        else
        {
            this.timeSinceRightPressStarted = 0f;
        }

        // This key makes a tick to an important one
        if (Input.GetKeyDown(KeyCode.W))
        {
            this.trackRythmUnits[this.currentRythmUnit] = true;
            this.mapUI.UpdateUI(this.trackRythmUnits, this.currentRythmUnit);
        }
        // This key removes the important flag from a key
        if (Input.GetKeyDown(KeyCode.S))
        {
            this.trackRythmUnits[this.currentRythmUnit] = false;
            this.mapUI.UpdateUI(this.trackRythmUnits, this.currentRythmUnit);
        }
    }

    /**
     * Moves the UI one step to the right
     */
    private void StepToTheRight()
    {
        // Check if the map contains a tick greater then the current one
        if (!(this.currentRythmUnit + 1 < this.trackRythmUnits.Length))
            return;

        // Increase current tick count
        this.currentRythmUnit++;

        // Increases the UI Pointer
        this.mapUI.IncreaseUIPointer();

        // Update the positions of the UI
        this.mapUI.UpdateUI(this.trackRythmUnits, this.currentRythmUnit);
    }

    /**
     * Moves the UI one step to the left
     */
    private void StepToTheLeft()
    {
        // Check if the map contains a tick less then the current one
        if (!(this.currentRythmUnit - 1 >= 0))
            return;

        // Decrease the current tick count
        this.currentRythmUnit--;

        // Decreases the UI pointer
        this.mapUI.DecreaseUIPointer();

        // Update the positions of the UI
        this.mapUI.UpdateUI(this.trackRythmUnits, this.currentRythmUnit);
    }

    #endregion UPDATE

    #region MAPDATA

    /**
     * Exports the map into a text file in json format
     */
    public void SaveMap()
    {
        // Create data object
        var data = new MapData()
        {
            trackBPM = this.trackBPM,
            offset = this.offset,
            markableTicksPerBeat = this.markableTicksPerBeat,

            visibleBeats = this.visibleBeats,
            beatSize = this.beatSize,
            tickSize = this.tickSize,

            trackRythmUnits = this.trackRythmUnits
        };

        // Save data object into file
        FileHandler.SaveObjectAsJsonString(this.trackSource.clip.name + "_" + this.saveName + ".txt", data);
    }

    /**
     * Imports the map from a text file in json format
     */
    public void LoadMap()
    {
        // First get the data
        var data = FileHandler.LoadObjectFromJsonString<MapData>(this.loadName);

        // Replace current data
        this.trackBPM = data.trackBPM;
        this.offset = data.offset;
        this.markableTicksPerBeat = data.markableTicksPerBeat;

        this.visibleBeats = data.visibleBeats;
        this.beatSize = data.beatSize;
        this.tickSize = data.tickSize;

        this.trackRythmUnits = data.trackRythmUnits;
        
        // need to update the UI and recalculate the trackTicks array if the game is running
        if (Application.isPlaying)
        {
            this.currentRythmUnit = 0;
            this.mapUI.DestroyUI();
            this.mapUI.SetupUI(this.visibleBeats, this.markableTicksPerBeat, this.beatSize, this.tickSize);
            this.mapUI.UpdateUI(this.trackRythmUnits, this.currentRythmUnit);
        }
    }

    #endregion MAPDATA

    #endregion METHODS
}
