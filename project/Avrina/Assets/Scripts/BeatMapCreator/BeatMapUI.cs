using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class BeatMapUI
{
    // Used to display the current map
    // It handels everything that has to do with drawing the ui
    private GameObject beatMapUI;
    // Reference Screen resolution the UI is working with (const)
    private Vector2 refRes = new Vector2(1600, 900);
    // Stores all ui elements representing ticks and beats
    private GameObject[] uiElements;
    // Current UIElement Pointer
    private int uiPointer;
    // Distance between two UI elements
    private float uiDistance;
    // Amout of UI Element to the left from the center of the Screen
    private int elementsLeftFromCenter;

    /**
     * creates the mother UI and set its parent object
     */
    public BeatMapUI(BeatMapCreator parentObject)
    {
        this.CreateUIContainer();
        this.beatMapUI.transform.SetParent(parentObject.transform, false);
    }

    /**
     * Creates the panel which contains all other UI elements
     */
    private void CreateUIContainer()
    {
        // Create the beat map ui and add required components
        this.beatMapUI = new GameObject("Beat Map UI");
        this.beatMapUI.AddComponent<RectTransform>();
        this.beatMapUI.AddComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
        // The scaler component need some changes because
        // We need to be sure that the design does always look the same
        var scaler = this.beatMapUI.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = this.refRes;
    }
    
    /**
     * Creates the beats and ticks for the UI and add them to the UIElements list
     */
    public void SetupUI(int visibleBeats, int markableTicksPerBeat, Vector2 beatSize, Vector2 tickSize)
    {
        // Define the amout of beats necessary to ensure the array has enough to work for each case
        var amoutOfBeats = visibleBeats + 1;

        // Calculate the amout of UI Elements necessary
        var amoutOfUIElements = amoutOfBeats + markableTicksPerBeat * amoutOfBeats;

        // Create the UIArray which stores all UI-Elements representing ticks and beats
        this.uiElements = new GameObject[amoutOfUIElements];

        // Generate UI-Elements
        var elementCounter = 0;
        for (var beatsCount = 0; beatsCount < amoutOfBeats; beatsCount++)
        {
            this.uiElements[elementCounter] = this.CreateUIElement("Beat", beatSize);
            elementCounter++;

            for (var ticksCount = 0; ticksCount < markableTicksPerBeat; ticksCount++)
            {
                this.uiElements[elementCounter] = this.CreateUIElement("Tick", tickSize);
                elementCounter++;
            }
        }

        // Calculate the amout of visible rythm units at the same time
        var visibleTicks = markableTicksPerBeat * amoutOfBeats;
        var visibleUIElements = visibleBeats + visibleTicks;
        // Distance between two rythm units
        this.uiDistance = this.refRes.x / visibleUIElements;

        // Setup the ui pointer
        this.uiPointer = 0;

        // Calculate the amout of elements to the left from the center
        // The variable is necessary for the UpdateUI Method
        var beatsLeftFromCenter = visibleBeats - 1;
        var ticksLeftFromCenter = markableTicksPerBeat + Mathf.FloorToInt(markableTicksPerBeat / 2f) * (visibleBeats - 1);
        this.elementsLeftFromCenter = beatsLeftFromCenter + ticksLeftFromCenter;
    }

    /**
     * Creates an UI Element with given size
     */
    private GameObject CreateUIElement(string type, Vector2 size)
    {
        // Create instance and set parent
        var ob = new GameObject(type);
        ob.transform.SetParent(this.beatMapUI.transform, false);
        // Set size of UI Element
        ob.transform.localScale = size;
        // Add an image and set background color to black
        var image = ob.AddComponent<Image>();
        image.color = Color.black;
        // Return the UI Element
        return ob;
    }

    /**
     * Used to destroy all UI element except the UIContainer
     */
    public void DestroyUI()
    {
        // Remove all UIElements
        foreach (var ob in this.uiElements)
        {
            Object.Destroy(ob);
        }
    } 

    /**
     * Uses the UIPointer as reference to place the UIElements relative to the center of the screen
     */
    public void UpdateUI(bool[] trackTicks, int currentTick)
    {
        // Set all Rythm Units invisible
        foreach (var ob in this.uiElements)
        {
            ob.SetActive(false);
        }
        
        // Sets color and position of the middle rythm unit
        this.UpdateUIElement(this.uiPointer, new Vector2(0, 0), trackTicks[currentTick] ? Color.red : Color.black);

        // Place all UI Element beginning from to the middle
        // Because its symmetrical we can place left and right at the same time
        for (var elementCount = 1; elementCount <= elementsLeftFromCenter; elementCount++)
        {
            // All UI Elements to the right are updated here
            // Checks if the rythm unit is visible
            if (currentTick + elementCount < trackTicks.Length)
            {
                // Set the UI Pointer position
                var tmpUIPointer = this.uiPointer + elementCount;
                // if the distance is bigger then the amout of UI Elements go to the left side of the array
                if (tmpUIPointer >= this.uiElements.Length)
                    tmpUIPointer -= this.uiElements.Length;

                // Update the UI Element and place it to the right position with the right color
                this.UpdateUIElement(
                    tmpUIPointer,
                    new Vector2(elementCount * this.uiDistance, 0),
                    (trackTicks[currentTick + elementCount]) ? Color.red : Color.black
                );
            }

            // All UI Elements to the left are updated here
            // Checks if the rythm unit is visible
            if (currentTick - elementCount >= 0)
            {
                // Set the UI Pointer position
                int tmpUIPointer = this.uiPointer - elementCount;
                // If the distance is smaller then 0 go to the right side of the array
                if (tmpUIPointer < 0)
                    tmpUIPointer += this.uiElements.Length;

                // Update the UI Element and place it to the right position with the right color
                this.UpdateUIElement(
                    tmpUIPointer,
                    new Vector2(-elementCount * this.uiDistance, 0),
                    (trackTicks[currentTick - elementCount]) ? Color.red : Color.black
                );
            }
        }
    }

    /**
     * Sets the UI Element at given index to active
     * and updates the position.
     * Also updates the color of the UI Element to given color
     */
    private void UpdateUIElement(int index, Vector2 position, Color color)
    {
        // Get the UI Element
        var uiElement = this.uiElements[index];

        // Set UI Element active
        uiElement.SetActive(true);

        // Set position
        uiElement.transform.localPosition = position;

        // Set color
        var image = (Image)uiElement.GetComponent<Image>();
        image.color = color;
    }

    /**
     * Increases the UI Pointer related to the UIElements array.
     * If the pointer would be outside the posible indezes increase or decrease him corresponding to the UIElements array
     */ 
    public void IncreaseUIPointer()
    {
        this.uiPointer++;
        if (this.uiPointer >= this.uiElements.Length)
        {
            this.uiPointer = 0;
        }
    }
    
    /**
     * Decreases the UI Pointer related to the UIElements array.
     * If the pointer would be outside the posible indezes increase or decrease him corresponding to the UIElements array
     */
    public void DecreaseUIPointer()
    {
        this.uiPointer--;
        if (this.uiPointer < 0)
        {
            this.uiPointer += this.uiElements.Length;
        }
    }
}
