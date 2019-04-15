using System.IO;
using UnityEngine;
using UnityEngine.UI;
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
    private AudioSource track;

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

    #region UIINTERN

    // used to display the current map
    // it handels everything that has to do with drawing the ui
    private GameObject beatMapUI;
    // stores the information how many posible beats can be marked as important
    // and which of them are important
    private List<bool> trackTicks;
    // stores all ui elements representing ticks and beats
    private List<GameObject> uiElements;
    // current UIElement Pointer
    private int uiPointer;
    // distance between two UI elements
    // (between two beats or ticks)
    private float uiDistance;
    // where is the current position in the song
    // (what part of the song need to be played next and what is the next tick)
    private int currentTick;

    #endregion UIINTERN

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

    #region CONSTANTS

    // reference Screen resolution the UI is working with (const)
    private Vector2 refRes = new Vector2(1600, 900);

    #endregion CONSTANTS

    #endregion VARIABLES

    #region METHODS

    #region SETUP

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
        // setup the UI Container which stores all ticks and beats
        this.createUIContainer();
        // setup the UI panels (they represent the ticks and beats)
        this.setupUI();
        // place the UI Panels on the screen
        this.updateUI();
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
    private void createTrackTicksList()
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

    /**
     * contains all UI Elements
     */
    private void createUIContainer()
    {
        // create the beat map ui and add required components
        this.beatMapUI = new GameObject("Beat Map UI");
        this.beatMapUI.transform.SetParent(this.transform, false);
        this.beatMapUI.AddComponent<RectTransform>();
        this.beatMapUI.AddComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
        // the scaler component need some changes because
        // we need to be sure that the design does always look the same
        CanvasScaler scaler = this.beatMapUI.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = this.refRes;
    }

    /**
     * creates the UI Element list an calculates some variables from given parameters via unity UI
     */
    private void setupUI()
    {
        // create the UIArray which stores all UI-Elements representing ticks and beats
        this.uiElements = new List<GameObject>();

        // generate UI-Elements
        for (int beatsCount = 0; beatsCount < this.visibleBeats + 1; beatsCount++)
        {
            this.addElementToUI("Beat");

            for (int ticksCount = 0; ticksCount < this.markableTicksPerBeat; ticksCount++)
            {
                this.addElementToUI("Tick");
            }
        }

        // calculate the amout of visible beats at the same time
        int visibleTicks = this.markableTicksPerBeat * (visibleBeats + 1);
        int visibleUIElements = this.visibleBeats + visibleTicks;
        // distance between two beats or a beat and a tick;
        this.uiDistance = this.refRes.x / visibleUIElements;

        // setup the ui pointer
        this.uiPointer = 0;
    }
    
    /**
     * adds an Keybeat to the UIElements list
     * if isTick is true the beat will be marked as a tick and therefore gets the tick size instead
     */
    private void addElementToUI(string type)
    {
        // create instance and set parent
        GameObject ob = new GameObject(type);
        ob.transform.SetParent(this.beatMapUI.transform, false);
        // create beat / tick
        switch (type)
        {
            case "Tick":
                ob.transform.localScale = this.tickSize;
                break;
            case "Beat":
                ob.transform.localScale = this.beatSize;
                break;
        }
        // add an image and set background color to black
        Image image = ob.AddComponent<Image>();
        image.color = Color.black;
        // add it to the UI List
        this.uiElements.Add(ob);
    }

    #endregion SETUP

    #region METHODSFOREDITOR

    /**
     * used from the editor to recalculate variables each time the user changes an input
     */
    public void recalculateUIElements()
    {
        // remove all UIElements
        foreach (GameObject ob in this.uiElements)
        {
            Destroy(ob);
        }
        // create tick map for the given song
        this.createTrackTicksList();
        // setup the UI panels (they represent the ticks and beats)
        this.setupUI();
        // place the UI Panels on the screen
        this.updateUI();
    }

    #endregion METHODSFOREDITOR

    #region UPDATE

    /**
     * default Unity update method
     */
    private void Update()
    {
        this.handleKeyInputs();

        if (this.track.isPlaying)
        {
            this.timeSinceLastRythemUnit += Time.deltaTime;
            if (this.timeSinceLastRythemUnit >= this.rythemUnitFrequency)
            {
                this.timeSinceLastRythemUnit -= this.rythemUnitFrequency;
                this.currentTick++;
                if (this.trackTicks[this.currentTick])
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
                this.currentTick = 0;
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
            if (this.timeSinceLeftPressStarted > 0.7f)
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
            if (this.timeSinceRightPressStarted > 0.7f)
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
            this.updateUI();
        }
        // this key removes the important flag from a key
        if (Input.GetKeyDown(KeyCode.S))
        {
            this.trackTicks[this.currentTick] = false;
            this.updateUI();
        }
    }

    /**
     * uses the UIPointer as reference to place the UIElements relative to the center of the screen
     */ 
    private void updateUI()
    {
        // first calculate the amout of elements to the left from the center
        // it is necessary because they are only drawn if the currentTick variable is high enough
        int beatsLeftFromCenter = this.visibleBeats - 1;
        int ticksLeftFromCenter = this.markableTicksPerBeat + Mathf.FloorToInt(this.markableTicksPerBeat / 2f) * (this.visibleBeats - 1);
        int elementsLeftFromCenter = beatsLeftFromCenter + ticksLeftFromCenter;

        // reset the position of the beats
        foreach (GameObject ob in this.uiElements)
        {
            ob.transform.localPosition = this.refRes;
        }

        // set color of the middle tick / beat
        this.updateUIColor(this.uiPointer, this.currentTick);
        // place the middle beat / tick (always visible)
        this.uiElements[this.uiPointer].transform.localPosition = new Vector2(0, 0);

        // place all elements to the left if they exist inside the map
        for (int elementCount = 1; elementCount <= elementsLeftFromCenter; elementCount++)
        {
            // checks if the map has a tick there if not break
            // (dont show ticks that arent inside the map)
            if (this.currentTick - elementCount < 0)
                break;

            // set the uiPointer location
            int tmpUIPointer = this.uiPointer - elementCount;
            // if the distance is smaller then 0 go to the right side of the array
            if (tmpUIPointer < 0)
                tmpUIPointer += this.uiElements.Count;

            // set color
            this.updateUIColor(tmpUIPointer, this.currentTick - elementCount);
            // set the position on the screen
            this.uiElements[tmpUIPointer].transform.localPosition = new Vector2(-elementCount * this.uiDistance, 0);
        }

        // place all elements to the right if they exist inside the map
        // because its symmetrical we can reuse the elementsLeftFromCenter variable
        for (int elementCount = 1; elementCount <= elementsLeftFromCenter; elementCount++)
        {
            // checks if the map has a tick there if not break
            // (dont show ticks that arent inside the map)
            if (this.currentTick + elementCount > this.trackTicks.Count)
                break;

            // set the uiPointer location
            int tmpUIPointer = this.uiPointer + elementCount;
            // if the distance is smaller then 0 go to the right side of the array
            if (tmpUIPointer >= this.uiElements.Count)
                tmpUIPointer -= this.uiElements.Count;

            // set color
            this.updateUIColor(tmpUIPointer, this.currentTick + elementCount);
            // set the position on the screen
            this.uiElements[tmpUIPointer].transform.localPosition = new Vector2(elementCount * this.uiDistance, 0);
        }
    }

    /**
     * updates the color of an UI Element to the corresponding tick in tracksticks
     */ 
    private void updateUIColor(int uiPointer, int tickPointer)
    {
        Image image = (Image)this.uiElements[uiPointer].GetComponent<Image>();

        if (this.trackTicks[tickPointer])
            image.color = Color.red;
        else
            image.color = Color.black;
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

        // increase the pointer to the current tick / beat
        this.uiPointer++;
        if (this.uiPointer >= this.uiElements.Count)
            this.uiPointer = 0;

        // update the positions of the UI
        this.updateUI();
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

        // decrease the pointer to the current tick / beat
        this.uiPointer--;
        if (this.uiPointer < 0)
            this.uiPointer += this.uiElements.Count;

        // update the positions of the UI
        this.updateUI();
    }

    #endregion UPDATE

    #region MAPDATA

    /**
     * exports the map into a text file in json format
     */
    private void saveMap()
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

        // save map
        string dataPath = Path.Combine(Application.persistentDataPath, this.track.clip.name + ".txt");
        string jsonString = JsonUtility.ToJson(data);
        using (StreamWriter streamWriter = File.CreateText(dataPath))
        {
            streamWriter.Write(jsonString);
        }

        /* LOAD DATA
        using (StreamReader streamReader = File.OpenText(path))
        {
            string jsonString = streamReader.ReadToEnd();
            return JsonUtility.FromJson<CharacterData>(jsonString);
        }
        */
    }

    #endregion MAPDATA

    #endregion METHODS
}
