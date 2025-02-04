﻿using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomInputModule : PointerInputModule
{
    /// <summary>
    ///  Define which types of input modes can the input module handle
    /// </summary>
    public enum InputMode
    {
        Mouse,
        Buttons
    }

    [SerializeField] private GraphicRaycaster canvasRaycastObject;
    /// <summary>
    ///  How many inputs can occour per second
    /// </summary>
    [SerializeField] private GameObject firstObjectToBeSelected;
    /// <summary>
    ///  How many inputs can occour per second
    /// </summary>
    [SerializeField] private float timeTillNextInput = 0.1f;
    /// <summary>
    ///  What will happen in the back event button is pressed
    /// </summary>
    [SerializeField] private UnityEvent backEvent;
    /// <summary>
    ///  Horizontal keyboard input axis
    /// </summary>
    [SerializeField] private string horizontalKeyBoardInput;
    /// <summary>
    ///  Horizontal joystick input axis
    /// </summary>
    [SerializeField] private string horizontalJoyStickInput;
    /// <summary>
    ///  Vertical keyboard input axis
    /// </summary>
    [SerializeField] private string verticalKeyBoardInput;
    /// <summary>
    ///  Vertical joystick input axis
    /// </summary>
    [SerializeField] private string verticalJoyStickInput;
    /// <summary>
    ///  Accept button on the keyboard
    /// </summary>
    [SerializeField] private KeyCode keyBoardAccept;
    /// <summary>
    ///  Accept button on the joystick
    /// </summary>
    [SerializeField] private KeyCode joyStickAccept;
    /// <summary>
    ///  Back button on the keyboard
    /// </summary>
    [SerializeField] private KeyCode keyBoardBack;
    /// <summary>
    ///  Back button on the joystick
    /// </summary>
    [SerializeField] private KeyCode joyStickBack;
    /// <summary>
    ///  Stores the hover sound effect
    /// </summary>
    [SerializeField] private AudioClip hoverSoundClip = null;
    /// <summary>
    ///  Stores the click sound effect
    /// </summary>
    [SerializeField] private DestroyAfterPlaying clickSound = null;
    /// <summary>
    ///  Position to play the hover audio sound from
    /// </summary>
    [SerializeField] private Vector3 audioPosition = Vector3.zero;
    /// <summary>
    ///  Used to calculate the mouse position relativ to the ui
    /// </summary>
    [SerializeField] private Canvas mouseTranslationCanvas = null;
    /// <summary>
    ///  How big steps will be made each update tick for the slider
    /// </summary>
    [SerializeField] private float stepSizeSlider = 0.01f;

    /// <summary>
    ///  Stores the current input method
    /// </summary>
    private InputMode currentInputMethod = InputMode.Buttons;
    /// <summary>
    ///  Stores the mouse position during the last update
    /// </summary>
    private Vector2 lastMousePos;
    /// <summary>
    ///  Stores the current mouse position
    /// </summary>
    private Vector2 currentMousePos;
    /// <summary>
    ///  Stores the last selected gameobject
    /// </summary>
    private GameObject lastSelected;
    /// <summary>
    ///  Stores the last selected gameobject
    /// </summary>
    private bool isSliderSelected;
    /// <summary>
    ///  Stores the last selected gameobject
    /// </summary>
    private Slider selectedSlider;
    /// <summary>
    ///  When was the last movement input handled
    /// </summary>
    private float lastMoveInput = 0f;


    /// <summary>
    ///  Handels all inputs and updates the event system
    /// </summary>
    public override void Process()
    {
        if (this.isSliderSelected && (Input.GetKey(this.keyBoardAccept) || Input.GetKey(this.joyStickAccept) || Input.GetMouseButton(0)))
        {
            return;
        }

        bool isWritingToObject = this.SendUpdateEventToSelectedObject();

        this.ListenForInputTypeChange();
        
        switch (this.currentInputMethod)
        {
            case InputMode.Mouse:
                this.HandleMouseInputs();
                break;
            case InputMode.Buttons:
                this.HandleButtonInputs(isWritingToObject);
                break;
        }
        
        if (Input.GetKeyDown(this.joyStickBack) || Input.GetKeyDown(this.keyBoardBack))
        {
            if (!AudioStorage.areSoundEffectsMuted)
            {
                Instantiate(this.clickSound).Play();
            }

            this.backEvent.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            var axisEventData = GetAxisEventData(0, -1, 0.6f);
            ExecuteEvents.Execute(this.eventSystem.currentSelectedGameObject, axisEventData, ExecuteEvents.moveHandler);
        }
    }

    /// <summary>
    ///  Sends all update events to the selected object first before processing own actions
    /// </summary>
    private bool SendUpdateEventToSelectedObject()
    {
        if (this.eventSystem.currentSelectedGameObject == null)
        {
            return false;
        }

        var data = GetBaseEventData();
        ExecuteEvents.Execute(this.eventSystem.currentSelectedGameObject, data, ExecuteEvents.updateSelectedHandler);

        return data.used;
    }

    /// <summary>
    ///  Will be called if the input mode is set to mouse.
    ///  Handles all mouse inputs
    /// </summary>
    private void HandleMouseInputs()
    {
        var mouseData = GetMousePointerEventData();
        var leftButtonData = mouseData.GetButtonState(PointerEventData.InputButton.Left);
        var pointerEvent = leftButtonData.eventData.buttonData;

        var currentOverGo = pointerEvent.pointerCurrentRaycast.gameObject;
        var selectHandlerGo = ExecuteEvents.GetEventHandler<ISelectHandler>(currentOverGo);
        this.eventSystem.SetSelectedGameObject(selectHandlerGo, pointerEvent);

        if (Input.GetMouseButtonDown(0))
        {
            var data = GetBaseEventData();
            var selectedObject = this.eventSystem.currentSelectedGameObject;
            if (selectedObject == null)
            {
                return;
            }

            if (!AudioStorage.areSoundEffectsMuted)
            {
                Instantiate(this.clickSound).Play();
            }

            if (!ExecuteEvents.Execute(selectedObject, data, ExecuteEvents.submitHandler))
            {
                this.selectedSlider = selectedObject.GetComponent<Slider>();
                this.isSliderSelected = true;
            }
        }
    }

    /// <summary>
    ///  Selects a gameobject if no gameobject was selected
    /// </summary>
    /// <returns>Returns if it was necessary to select a gameobject</returns>
    private bool SetSelectedGameObject()
    {
        if (this.eventSystem.currentSelectedGameObject == null)
        {
            this.eventSystem.SetSelectedGameObject(this.firstObjectToBeSelected);

            return true;
        }

        return false;
    }

    /// <summary>
    ///  Will be called if the input mode is set to buttons.
    ///  Handles all keyboard / joystick inputs
    /// </summary>
    private void HandleButtonInputs(bool isWritingToObject)
    {
        if (this.SetSelectedGameObject())
        {
            return;
        }

        var data = GetBaseEventData();
        if (Input.GetKeyDown(this.keyBoardAccept) || Input.GetKeyDown(this.joyStickAccept))
        {
            if (!AudioStorage.areSoundEffectsMuted)
            {
                Instantiate(this.clickSound).Play();
            }

            var selectedObject = this.eventSystem.currentSelectedGameObject;
            if (!ExecuteEvents.Execute(selectedObject, data, ExecuteEvents.submitHandler))
            {
                this.selectedSlider = selectedObject.GetComponent<Slider>();
                this.isSliderSelected = true;
            }
        }
        else
        {
            var movement = GetRawMoveVector(isWritingToObject);
            var axisEventData = GetAxisEventData(movement.x, movement.y, 0.6f);
            if (
                !Mathf.Approximately(axisEventData.moveVector.x, 0f) 
                || !Mathf.Approximately(axisEventData.moveVector.y, 0f)
            ) {
                if (Time.time < this.lastMoveInput)
                {
                    return;
                }

                ExecuteEvents.Execute(this.eventSystem.currentSelectedGameObject, axisEventData, ExecuteEvents.moveHandler);
                this.lastMoveInput = Time.time + this.timeTillNextInput;
            }
            else
            {
                this.lastMoveInput = 0f;
            }
        }
    }

    /// <summary>
    ///  Builds movement vector from the keyboard / joystick inputs
    /// </summary>
    /// <returns></returns>
    private Vector2 GetRawMoveVector(bool isWritingToObject)
    {
        var move = Vector2.zero;
        
        move.x = Input.GetAxisRaw(this.horizontalJoyStickInput);
        move.y = Input.GetAxisRaw(this.verticalJoyStickInput);

        if (!isWritingToObject)
        {
            if (Mathf.Approximately(move.x, 0.0f))
            {
                move.x = Input.GetAxisRaw(this.horizontalKeyBoardInput);
            }
            if (Mathf.Approximately(move.y, 0.0f))
            {
                move.y = Input.GetAxisRaw(this.verticalKeyBoardInput);
            }
        }

        return move;
    }

    /// <summary>
    ///  Checks if the other input type is used and changes the state accordingly
    /// </summary>
    private void ListenForInputTypeChange()
    {
        if (Input.GetMouseButton(0) || this.lastMousePos != this.currentMousePos)
        {
            this.currentInputMethod = InputMode.Mouse;
        }

        if (
            !Mathf.Approximately(Input.GetAxisRaw(this.verticalJoyStickInput), 0.0f)
            || !Mathf.Approximately(Input.GetAxisRaw(this.verticalKeyBoardInput), 0.0f)
            || !Mathf.Approximately(Input.GetAxisRaw(this.horizontalJoyStickInput), 0.0f)
            || !Mathf.Approximately(Input.GetAxisRaw(this.horizontalKeyBoardInput), 0.0f)
            || Input.GetKey(this.joyStickAccept)
            || Input.GetKey(this.keyBoardAccept)
            || Input.GetKey(this.joyStickBack)
            || Input.GetKey(this.keyBoardBack)
        ) {
            this.currentInputMethod = InputMode.Buttons;
        }
    }

    /// <summary>
    ///  Checks if the current selected gameobject changed
    /// </summary>
    public void Update()
    {
        if (this.isSliderSelected && (Input.GetKey(this.keyBoardAccept) || Input.GetKey(this.joyStickAccept) || Input.GetMouseButton(0)))
        {
            switch (this.currentInputMethod)
            {
                case InputMode.Buttons:
                    var direction = Input.GetAxisRaw(this.horizontalJoyStickInput);
                    if (Time.time < this.lastMoveInput)
                    {
                        if (Mathf.Abs(direction) < 0.2)
                        {
                            this.lastMoveInput = Time.time;
                        }

                        return;
                    }

                    if (direction > 0.4f)
                    {
                        var currentSliderValue = this.selectedSlider.value + this.stepSizeSlider;
                        if (currentSliderValue > 1)
                        {
                            this.selectedSlider.value = 1;
                        }
                        else
                        {
                            this.selectedSlider.value = currentSliderValue;
                        }

                        this.lastMoveInput = Time.time + this.timeTillNextInput;
                    }
                    else if (direction < -0.4f)
                    {
                        var currentSliderValue = this.selectedSlider.value - this.stepSizeSlider;
                        if (currentSliderValue < 0)
                        {
                            this.selectedSlider.value = 0;
                        }
                        else
                        {
                            this.selectedSlider.value = currentSliderValue;
                        }

                        this.lastMoveInput = Time.time + this.timeTillNextInput;
                    }

                    break;
                case InputMode.Mouse:
                    Vector2 movePos;

                    RectTransformUtility.ScreenPointToLocalPointInRectangle(
                        this.mouseTranslationCanvas.transform as RectTransform,
                        Input.mousePosition,
                        this.mouseTranslationCanvas.worldCamera,
                        out movePos
                    );

                    var mousePosInUi = this.mouseTranslationCanvas.transform.TransformPoint(movePos);
                    var sliderSizeX = (this.selectedSlider.transform as RectTransform).sizeDelta.x;
                    this.selectedSlider.value = Mathf.Clamp((mousePosInUi.x - this.selectedSlider.transform.position.x + sliderSizeX * 0.5f) / sliderSizeX, 0, 1);
                    break;
            }

            return;
        }
        this.isSliderSelected = false;

        if (this.lastSelected == null)
        {
            this.lastSelected = this.eventSystem.currentSelectedGameObject;
        }

        if (this.lastSelected != this.eventSystem.currentSelectedGameObject)
        {
            this.lastSelected = this.eventSystem.currentSelectedGameObject;

            if (!AudioStorage.areSoundEffectsMuted)
            {
                AudioSource.PlayClipAtPoint(this.hoverSoundClip, this.audioPosition, AudioStorage.soundEffectVolume);
            }
        }
    }

    /// <summary>
    ///  Store the mouse positions
    /// </summary>
    public override void UpdateModule()
    {
        this.lastMousePos = this.currentMousePos;
        this.currentMousePos = Input.mousePosition;
    }
}
