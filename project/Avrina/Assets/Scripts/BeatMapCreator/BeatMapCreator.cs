using UnityEngine;
using System.Collections.Generic;

public class BeatMapCreator : MonoBehaviour
{
    #region VARIABLES

    #region SOUND

    // tick sound source
    public AudioClip tickSource;
    // tick Sound player
    private AudioSource tickSound;
    // song the map is created for
    public AudioClip trackSource;
    // song as playable element
    public AudioSource track;

    #endregion SOUND

    #region MAPSPECIFICATIONS

    // bpm of the song the map is created for
    public float trackBPM;
    // how long does it take until the first beat of the song is played
    public float offset;
    // how many ticks are per beat
    // defines how many part per beat can be marked as important.
    // (how big is the data of the map)
    public int markableTicksPerBeat;

    #endregion MAPSPECIFICATIONS

    #region UISPECIFICATIONS

    // it defines how much of the map is currently visible
    public int visibleBeats;
    // defines how big the beat panel in reference to the display size is (ratio)
    public Vector2 beatSize;
    // defines how big the tick panel in reference to the display size is (ratio)
    public Vector2 tickSize;

    #endregion UISPECIFICATIONS

    #region UPDATE

    // defines how much time passed since the button was stared pressed down
    private float timeSinceLeftPressStarted;
    // defines how much time passed since the button was stared pressed down
    private float timeSinceRightPressStarted;
    // time since the last rythem unit
    private float timeSinceLastRythemUnit;
    // stores how long it takes until the next rythem unit has to be played
    private float rythemUnitFrequency;

    #endregion UPDATE

    #region PLAYSONG

    // used to get the offset of the song right for the tick sounds
    private float tmpOffset;
    // because we need to current ticks we need to replace one
    private int tmpCurrentTick;

    #endregion PLAYSONG

    #region MAPDATA

    // name of the file the song is stored into
    public string saveName;
    // name of the file the song is loaded from
    public string loadName;

    #endregion MAPDATA

    #region MAPINFO

    // stores the information how many posible beats can be marked as important
    // and which of them are important
    public List<bool> trackTicks;
    // where is the current position in the song
    // (what part of the song need to be played next and what is the next tick)
    public int currentTick;

    #endregion MAPINFO

    #region UI

    // contains all UI elements and handels their movement over the screen
    public BeatMapUI beatMapUI;

    #endregion UI

    #endregion VARIABLES

    #region METHODS

    #region SETUP

    /**
     * called really early from the Unity System to prevent errors
     */
    private void OnEnable()
    {
        this.beatMapUI = new BeatMapUI(this);
    }

    /**
     * default Unity start method
     */
    private void Start()
    {
        // define some variables
        this.timeSinceLeftPressStarted = 0f;
        this.timeSinceRightPressStarted = 0f;

        // load the map sound and the tick sound
        this.setupSound();
        // create tick map for the given song
        this.createTrackTicksList();

        // setup the UI
        this.beatMapUI.setupUI(this.visibleBeats, this.markableTicksPerBeat, this.beatSize, this.tickSize);
        this.beatMapUI.updateUI(this.trackTicks, this.currentTick, this.visibleBeats, this.markableTicksPerBeat);
    }

    /**
     * loads all sound files
     */
    private void setupSound()
    {
        // create container for the audio player (sources)
        GameObject audioContainer = new GameObject("AudioContainer");
        audioContainer.transform.SetParent(this.transform, false);
        // load tick sound into the script
        this.tickSound = audioContainer.AddComponent<AudioSource>();
        this.tickSound.playOnAwake = false;
        this.tickSound.clip = this.tickSource;
        // load track into the script
        this.track = audioContainer.AddComponent<AudioSource>();
        this.track.playOnAwake = false;
        this.track.clip = this.trackSource;
        // set current play position to 0
        this.currentTick = 0;
    }

    /**
     * creates an List which contains all posible ticks for given song
     */
    public void createTrackTicksList()
    {
        // reset current Tick
        this.currentTick = 0;
        // create instance of map
        this.trackTicks = new List<bool>();
        // calculate the amout of ticks in the track
        float beatsPerSecond = this.trackBPM / 60;
        float numberOfBeats = this.track.clip.length * beatsPerSecond;
        int numberOfTicks = Mathf.RoundToInt(numberOfBeats * this.markableTicksPerBeat);

        // stores the amout of rythem units per second (beats and ticks per second)
        float ticksPerSecond = beatsPerSecond * this.markableTicksPerBeat;
        float rythemUnitsPerSecond = beatsPerSecond + ticksPerSecond;
        this.rythemUnitFrequency = 1 / rythemUnitsPerSecond;

        // create the map size corresponding to the amout of ticks in the track
        for (int i = 0; i < numberOfTicks; i++)
        {
            this.trackTicks.Add(false);
        }
    }
            
    #endregion SETUP

    #region UPDATE

    /**
     * default Unity update method
     */
    private void Update()
    {
        this.handleKeyInputs();

        if (this.track.isPlaying)
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
            if (this.timeSinceLastRythemUnit >= this.rythemUnitFrequency)
            {
                this.timeSinceLastRythemUnit -= this.rythemUnitFrequency;
                this.tmpCurrentTick++;
                if (this.trackTicks[this.tmpCurrentTick])
                {
                    this.tickSound.Play();
                }
            }
        }
    }

    /**
     * handles all key inputs
     */ 
    private void handleKeyInputs()
    {
        // saves the map as json
        if (Input.GetKeyDown(KeyCode.Q))
        {
            this.saveMap();
        }

        // this key starts or stops the track
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!this.track.isPlaying)
            {
                this.track.Play();
                this.timeSinceLastRythemUnit = 0;
                this.tmpCurrentTick = 0;
                this.tmpOffset = this.offset;
            } else
            {
                this.track.Stop();
            }
        }

        // this key goes a tick back in the song
        if (Input.GetKeyDown(KeyCode.A))
        {
            this.stepToTheLeft();
        }
        // this key goes a tick forward
        if (Input.GetKeyDown(KeyCode.D))
        {
            this.stepToTheRight();
        }
        // if the right key is pressed long enough the map will step fast to the right
        if (Input.GetKey(KeyCode.A))
        {
            this.timeSinceLeftPressStarted += Time.deltaTime;
            if (this.timeSinceLeftPressStarted > 0.4f)
            {
                this.stepToTheLeft();
            }
        }
        else
        {
            this.timeSinceLeftPressStarted = 0f;
        }
        // if the left key is pressed long enough the map will step fast to the left
        if (Input.GetKey(KeyCode.D))
        {
            this.timeSinceRightPressStarted += Time.deltaTime;
            if (this.timeSinceRightPressStarted > 0.4f)
            {
                this.stepToTheRight();
            }
        }
        else
        {
            this.timeSinceRightPressStarted = 0f;
        }

        // this key makes a tick to an important one
        if (Input.GetKeyDown(KeyCode.W))
        {
            this.trackTicks[this.currentTick] = true;
            this.beatMapUI.updateUI(this.trackTicks, this.currentTick, this.visibleBeats, this.markableTicksPerBeat);
        }
        // this key removes the important flag from a key
        if (Input.GetKeyDown(KeyCode.S))
        {
            this.trackTicks[this.currentTick] = false;
            this.beatMapUI.updateUI(this.trackTicks, this.currentTick, this.visibleBeats, this.markableTicksPerBeat);
        }
    }

    /**
     * moves the UI one step to the right
     */
    private void stepToTheRight()
    {
        // check if the map contains a tick greater then the current one
        if (!(this.currentTick < this.trackTicks.Count))
            return;

        // increase current tick count
        this.currentTick++;

        // increases the UI Pointer
        this.beatMapUI.increaseUIPointer();

        // update the positions of the UI
        this.beatMapUI.updateUI(this.trackTicks, this.currentTick, this.visibleBeats, this.markableTicksPerBeat);
    }

    /**
     * moves the UI one step to the left
     */
    private void stepToTheLeft()
    {
        // check if the map contains a tick less then the current one
        if (!(this.currentTick > 0))
            return;

        // decrease the current tick count
        this.currentTick--;

        // decreases the UI pointer
        this.beatMapUI.decreaseUIPointer();

        // update the positions of the UI
        this.beatMapUI.updateUI(this.trackTicks, this.currentTick, this.visibleBeats, this.markableTicksPerBeat);
    }

    #endregion UPDATE

    #region MAPDATA

    /**
     * exports the map into a text file in json format
     */
    public void saveMap()
    {
        // create data object
        MapData data = new MapData();

        data.trackBPM = this.trackBPM;
        data.offset = this.offset;
        data.markableTicksPerBeat = this.markableTicksPerBeat;

        data.visibleBeats = this.visibleBeats;
        data.beatSize = this.beatSize;
        data.tickSize = this.tickSize;

        data.trackTicks = this.trackTicks;

        // save data object into file
        FileHandler.saveObjectAsJsonString(this.track.clip.name + "_" + this.saveName + ".txt", data);
    }

    /**
     * imports the map from a text file in json format
     */
    public void loadMap()
    {
        // first get the data
        MapData data = FileHandler.loadObjectFromJsonString<MapData>(this.loadName);

        // replace current data
        this.trackBPM = data.trackBPM;
        this.offset = data.offset;
        this.markableTicksPerBeat = data.markableTicksPerBeat;

        this.visibleBeats = data.visibleBeats;
        this.beatSize = data.beatSize;
        this.tickSize = data.tickSize;

        this.trackTicks = data.trackTicks;
        
        // need to update the UI and recalculate the trackTicks array if the game is running
        if (Application.isPlaying)
        {
            this.currentTick = 0;
            this.beatMapUI.destroyUI();
            this.beatMapUI.setupUI(this.visibleBeats, this.markableTicksPerBeat, this.beatSize, this.tickSize);
            this.beatMapUI.updateUI(this.trackTicks, this.currentTick, this.visibleBeats, this.markableTicksPerBeat);
        }
    }

    #endregion MAPDATA

    #endregion METHODS
}
