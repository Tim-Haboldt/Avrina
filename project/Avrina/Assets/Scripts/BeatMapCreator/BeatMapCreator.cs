using UnityEngine;
using UnityEngine.UI;
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
    // how many tick per second
    private float ticksPerSecond;

    // used to display the current map
    // it handels everything that has to do with drawing the ui
    private GameObject beatMapUI;
    // it defines how much of the map is currently visible
    public int visibleBeats;
    // defines how big the beat panel in reference to the display size is (ratio)
    public Vector2 beatSize;
    // defines how big the tick panel in reference to the display size is (ratio)
    public Vector2 tickSize;

    // stores the information how many posible beats can be marked as important
    // and which of them are important
    private List<bool> trackTicks;
    // stores all ui elements representing ticks and beats
    private List<GameObject> uiElements;
    // current UIElement Pointer
    private int uiPointer;
    // stores the amount of visible ui elements at the same time
    private int visibleUIElements;
    // distance between two UI elements
    // (between two beats or ticks)
    private float uiDistance;
    // where is the current position in the song
    // (what part of the song need to be played next and what is the next tick)
    private int currentTick;

    // reference Screen resolution the UI is working with (const)
    private Vector2 refRes = new Vector2(1600, 900);

    /**
     * default Unity start method
     */
    private void Start()
    {
        // load the map sound and the tick sound
        this.setupSound();
        // create tick map for the given song
        this.createTrackTicksList();
        // setup the Ui panels (they represent the ticks and beats)
        this.setupBeatMapUI();
    }

    /**
     * default Unity update method
     */ 
    private void Update()
    {
        this.handleKeyInputs();
    }

    /**
     * handles all key inputs
     */ 
    private void handleKeyInputs()
    {
        // this key starts or stops the track
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!this.track.isPlaying)
            {
                this.track.Play();
            }
        }
        // this key goes a tick back in the song
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (!(this.currentTick > 0))
                return;

            this.currentTick--;
        }
        // this key goes a tick forward
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (!(this.currentTick < this.trackTicks.Count - 1))
                return;

            // increase current tick count
            this.currentTick++;
            // set the pointer to the ticks
            this.uiPointer++;
            if (this.uiPointer > this.trackTicks.Count - 1)
                this.uiPointer = 0;

            // get the amount of UIElements to the left from the middle one
            int elementsToTheLeft = Mathf.FloorToInt(this.visibleUIElements);
            // reset the uiElements position
            foreach (GameObject go in this.uiElements)
            {
                go.transform.localPosition = this.refRes;
            }
            // set the position of the UIElements
            for (int elementCount = 0; elementCount < this.visibleUIElements; elementCount++)
            {
                // check if there are ticks outside of the map spectrum (No need to place an ui element there)
                //if (!(this.currentTick - (elementsToTheLeft - elementCount) < 0)
                //    && !(this.currentTick + (elementCount - elementsToTheLeft) < this.trackTicks.Count - 1))
                //{
                    int arrayIndex = this.uiPointer - elementsToTheLeft + elementCount;
                    if (arrayIndex < 0)
                        arrayIndex += this.uiElements.Count - 1;
                    if (arrayIndex >= this.uiElements.Count)
                        arrayIndex -= this.uiElements.Count - 1;

                    this.uiElements[arrayIndex].transform.localPosition = new Vector2(elementCount * this.uiDistance, 0);
                //}
            }

            // distance between two beats or a beat and a tick;
            float elementDistance = this.refRes.x / (this.uiElements.Count - this.markableTicksPerBeat);
            // set new position of the UI elements
            foreach (GameObject ob in this.uiElements)
            {
                // calculate the new position
                float newPosX = ob.transform.localPosition.x - elementDistance;
                // check if the new position is outside of the sceen.
                float positionOutSide = newPosX + (this.refRes.x / 2);
                if (positionOutSide <= 0)
                {
                    // if so move the pos to the right side
                    // first calculate how many steps the element is outside the window
                    float stepsOutside = positionOutSide / elementDistance;
                    // now invert the stepsOutside value because the element is placed farther to the right if it wasn't as many steps outside
                    float invStepsOutside = (this.markableTicksPerBeat - 1) - stepsOutside;
                    // place it the right
                    newPosX += this.refRes.x + elementDistance * invStepsOutside;
                }
                // set new position
                ob.transform.localPosition = new Vector2(newPosX, 0);
            }
        }
    }

    /**
     * creates an List which contains all posible ticks for given song
     */
    private void createTrackTicksList()
    {
        // create instance of map
        this.trackTicks = new List<bool>();
        // calculate the amout of ticks in the track
        float bpmPerSecond = this.trackBPM / 60;
        float numberOfBeats = this.track.clip.length * bpmPerSecond;
        int numberOfTicks = Mathf.RoundToInt(numberOfBeats * this.markableTicksPerBeat);

        // stores the amout of ticks per second
        this.ticksPerSecond = bpmPerSecond * this.markableTicksPerBeat;

        // create the map size corresponding to the amout of ticks in the track
        for (int i = 0; i < numberOfTicks; i++)
        {
            this.trackTicks.Add(false);
        }
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
     * 
     */
    private void setupBeatMapUI()
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

        // create the UIArray which stores all UI-Elements representing ticks and beats
        this.uiElements = new List<GameObject>();
        
        // generate UI-Elements
        for (int beatsCount = 0; beatsCount < this.visibleBeats + 1; beatsCount++)
        {
            this.addElementToUI(false);

            for (int ticksCount = 0; ticksCount < this.markableTicksPerBeat; ticksCount++)
            {
                this.addElementToUI(true);
            }
        }

        // calculate the amout of visible beats at the same time
        int visibleTicks = (this.markableTicksPerBeat * visibleBeats) - 1;
        this.visibleUIElements = this.visibleBeats + visibleTicks;
        // distance between two beats or a beat and a tick;
        this.uiDistance = this.refRes.x / this.visibleUIElements;

        // set position of the ticks outside of the screen and in the right order
        for (int elementCount = 0; elementCount < this.uiElements.Count; elementCount++)
        {
            // calculate the distance where the element is placed
            // (later in the array more far behind)
            float xPos = elementCount * this.uiDistance;
            // set the new position
            // (the global position is always at the center. y is 0 because it should be centered)
            this.uiElements[elementCount].transform.localPosition = new Vector2(xPos, 0);
        }

        // setup the ui pointer
        this.uiPointer = 0;
    }

    /**
     * adds an Keybeat to the UIElements list
     * if isTick is true the beat will be marked as a tick and therefore gets the tick size instead
     */ 
    private void addElementToUI(bool isTick)
    {
        GameObject ob;
        // create beat / tick
        if (isTick)
        {
            ob = new GameObject("Tick");
            ob.transform.SetParent(this.beatMapUI.transform, false);
            // set size of tick
            ob.transform.localScale = this.tickSize;

        } else
        {
            ob = new GameObject("Beat");
            ob.transform.SetParent(this.beatMapUI.transform, false);
            // set size of beat
            ob.transform.localScale = this.beatSize;
        }
        // add an image and set background color to black
        Image image = ob.AddComponent<Image>();
        image.color = Color.black;
        // add it to the UI List
        this.uiElements.Add(ob);
    }
}
