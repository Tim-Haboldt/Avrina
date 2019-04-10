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
    // used to display the current map
    // it handels everything that has to do with drawing the ui
    private GameObject beatMapUI;
    // it defines how much of the map is currently visible
    public int visibleKeyBeats;
    // defines how big the beat panel in reference to the display size is (ratio)
    public Vector2 beatSize;
    // defines how big the tick panel in reference to the display size is (ratio)
    public Vector2 tickSize;
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
        // load the map sound and the tick sound
        this.setupSound();
        // create tick map for the given song
        this.createTrackTicksList();
        // setup the Ui panels (they represent the ticks and beats)
        this.setupBeatMapUI();

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
        scaler.referenceResolution.Set(1600, 900);

        GameObject[] beatsAndTicks = new GameObject[this.markableTicksPerBeat * this.visibleKeyBeats + this.visibleKeyBeats];
        // Screen.Width
        // create the panel
        GameObject panel = new GameObject("Panel");
        panel.transform.SetParent(beatMapUI.transform, false);
        // add an image and set background color to red
        Image i = panel.AddComponent<Image>();
        i.color = Color.red;
        i.transform.SetParent(panel.transform, false);
        panel.transform.localScale = this.beatSize;
        panel.transform.localPosition = new Vector2(panel.transform.localPosition.x + 102.0f, panel.transform.localPosition.y + 12.0f);
    }
        /*
    Mesh mesh;

    private Vector3[] vertecies;
    private int[] triangles;
    private void setupBeatMapUI()
    {
        // create container for the mesh renderer and filter
        GameObject meshContainer = new GameObject("MeshContainer");
        meshContainer.transform.SetParent(this.transform, true);
        // add mesh renderer and mesh filter
        meshContainer.AddComponent<MeshFilter>();
        meshContainer.AddComponent<MeshRenderer>();

        mesh = new Mesh();
        meshContainer.GetComponent<MeshFilter>().mesh = mesh;

        vertecies = new Vector3[]
        {
            new Vector3(0, 0, 0),
            new Vector3(0, 1, 0),
            new Vector3(1, 0, 0)
        };

        triangles = new int[]
        {
            0, 1, 2
        };

        mesh.Clear();
        mesh.vertices = vertecies;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
    }*/

}
