using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class BeatMapUI
{
    // used to display the current map
    // it handels everything that has to do with drawing the ui
    private GameObject beatMapUI;
    // reference Screen resolution the UI is working with (const)
    private Vector2 refRes = new Vector2(1600, 900);
    // stores all ui elements representing ticks and beats
    private List<GameObject> uiElements;
    // current UIElement Pointer
    private int uiPointer;
    // distance between two UI elements
    // (between two beats or ticks)
    private float uiDistance;

    /**
     * creates the mother UI and set its parent object
     */ 
    public BeatMapUI(BeatMapCreator parentObject)
    {
        this.createUIContainer();
        this.beatMapUI.transform.SetParent(parentObject.transform, false);
    }

    /**
     * creates the panel which contains all other UI elements
     */
    private void createUIContainer()
    {
        // create the beat map ui and add required components
        this.beatMapUI = new GameObject("Beat Map UI");
        this.beatMapUI.AddComponent<RectTransform>();
        this.beatMapUI.AddComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
        // the scaler component need some changes because
        // we need to be sure that the design does always look the same
        CanvasScaler scaler = this.beatMapUI.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = this.refRes;
    }
    
    /**
     * creates the beats and ticks for the UI and add them to the UIElements list
     */
    public void setupUI(int visibleBeats, int markableTicksPerBeat, Vector2 beatSize, Vector2 tickSize)
    {
        // create the UIArray which stores all UI-Elements representing ticks and beats
        this.uiElements = new List<GameObject>();

        // generate UI-Elements
        for (int beatsCount = 0; beatsCount < visibleBeats + 1; beatsCount++)
        {
            this.addElementToUI("Beat", beatSize);

            for (int ticksCount = 0; ticksCount < markableTicksPerBeat; ticksCount++)
            {
                this.addElementToUI("Tick", tickSize);
            }
        }

        // calculate the amout of visible beats at the same time
        int visibleTicks = markableTicksPerBeat * (visibleBeats + 1);
        int visibleUIElements = visibleBeats + visibleTicks;
        // distance between two beats or a beat and a tick;
        this.uiDistance = this.refRes.x / visibleUIElements;

        // setup the ui pointer
        this.uiPointer = 0;
    }

    /**
     * adds an Beat / Tick to the UIElements list
     */
    private void addElementToUI(string type, Vector2 size)
    {
        // create instance and set parent
        GameObject ob = new GameObject(type);
        ob.transform.SetParent(this.beatMapUI.transform, false);
        // create beat / tick
        ob.transform.localScale = size;
        // add an image and set background color to black
        Image image = ob.AddComponent<Image>();
        image.color = Color.black;
        // add it to the UI List
        this.uiElements.Add(ob);
    }

    /**
     * used to destroy all UI element except the UIContainer
     */
    public void destroyUI()
    {
        // remove all UIElements
        foreach (GameObject ob in this.uiElements)
        {
            Object.Destroy(ob);
        }
    } 

    /**
     * uses the UIPointer as reference to place the UIElements relative to the center of the screen
     */
    public void updateUI(List<bool> trackTicks, int currentTick, int visibleBeats, int markableTicksPerBeat)
    {
        // first calculate the amout of elements to the left from the center
        // it is necessary because they are only drawn if the currentTick variable is high enough
        int beatsLeftFromCenter = visibleBeats - 1;
        int ticksLeftFromCenter = markableTicksPerBeat + Mathf.FloorToInt(markableTicksPerBeat / 2f) * (visibleBeats - 1);
        int elementsLeftFromCenter = beatsLeftFromCenter + ticksLeftFromCenter;

        // reset the position of the beats
        foreach (GameObject ob in this.uiElements)
        {
            ob.transform.localPosition = this.refRes;
        }

        // set color of the middle tick / beat
        this.updateUIColor(trackTicks, currentTick, this.uiPointer);
        // place the middle beat / tick (always visible)
        this.uiElements[this.uiPointer].transform.localPosition = new Vector2(0, 0);

        // place all elements to the left if they exist inside the map
        for (int elementCount = 1; elementCount <= elementsLeftFromCenter; elementCount++)
        {
            // checks if the map has a tick there if not break
            // (dont show ticks that arent inside the map)
            if (currentTick - elementCount < 0)
                break;

            // set the uiPointer location
            int tmpUIPointer = this.uiPointer - elementCount;
            // if the distance is smaller then 0 go to the right side of the array
            if (tmpUIPointer < 0)
                tmpUIPointer += this.uiElements.Count;

            // set color
            this.updateUIColor(trackTicks, currentTick - elementCount, tmpUIPointer);
            // set the position on the screen
            this.uiElements[tmpUIPointer].transform.localPosition = new Vector2(-elementCount * this.uiDistance, 0);
        }

        // place all elements to the right if they exist inside the map
        // because its symmetrical we can reuse the elementsLeftFromCenter variable
        for (int elementCount = 1; elementCount <= elementsLeftFromCenter; elementCount++)
        {
            // checks if the map has a tick there if not break
            // (dont show ticks that arent inside the map)
            if (currentTick + elementCount > trackTicks.Count)
                break;

            // set the uiPointer location
            int tmpUIPointer = this.uiPointer + elementCount;
            // if the distance is smaller then 0 go to the right side of the array
            if (tmpUIPointer >= this.uiElements.Count)
                tmpUIPointer -= this.uiElements.Count;

            // set color
            this.updateUIColor(trackTicks, currentTick + elementCount, tmpUIPointer);
            // set the position on the screen
            this.uiElements[tmpUIPointer].transform.localPosition = new Vector2(elementCount * this.uiDistance, 0);
        }
    }
    
    /**
     * updates the color of an UI Element to the corresponding tick in tracksticks
     */
    private void updateUIColor(List<bool> trackTicks, int tickPointer, int uiPointer)
    {
        Image image = (Image)this.uiElements[uiPointer].GetComponent<Image>();

        if (trackTicks[tickPointer])
            image.color = Color.red;
        else
            image.color = Color.black;
    }

    /**
     * increases the UI Pointer related to the UIElements array.
     * If the pointer would be outside the posible indezes increase or decrease him corresponding to the UIElements array
     */ 
    public void increaseUIPointer()
    {
        this.uiPointer++;
        if (this.uiPointer >= this.uiElements.Count)
            this.uiPointer = 0;
    }
    
    /**
     * decreases the UI Pointer related to the UIElements array.
     * If the pointer would be outside the posible indezes increase or decrease him corresponding to the UIElements array
     */
    public void decreaseUIPointer()
    {
        this.uiPointer--;
        if (this.uiPointer < 0)
            this.uiPointer += this.uiElements.Count;
    }
}
