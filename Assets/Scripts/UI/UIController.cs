using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

public class UIController : MonoBehaviour
{
    // DESIGN CHOICE: Inject persistent input asset into UI controls so that it does not
    // rely on runtime reference to input (preventing it from running properly without its owner being instantiated)
    [Header("Input Channels")]
    [SerializeField] FirstPersonActionsSO playerInput;
    [Header("Output Channels")]
    [SerializeField] UIStateEventChannelSO stateChanged;

    [Header("Panel Animators")]
    [Tooltip("The pause panel")]
    [SerializeField] PanelAnimator pause;
    [Tooltip("The panel that will be brought to the center of attention")]
    [SerializeField] PanelAnimator main;
    [Tooltip("The background panel that provides contrast for the main panel")]
    [SerializeField] PanelAnimator mainBackground;

    // Player input source
    InputAction toggleMainAction;
    InputAction togglePauseAction;

    bool isMainOpen = false;
    bool isPauseOpen = false;



    void Awake()
    {
        // Initialize the actions
        toggleMainAction = playerInput.controls.Inventory;
        togglePauseAction = playerInput.controls.Pause;

        // Initialize background to be closed
        mainBackground.InstantClose();
    }

    void Start()
    {
        // Broadcast the initial state so that listeners can get initialized (in Start since they may subscribe during Awake)
        stateChanged.RaiseEvent(UIState.CLEAR);
    }

    void OnEnable()
    {
        toggleMainAction.performed += HandleToggleMain;
        togglePauseAction.performed += HandleTogglePause;
    }

    void OnDisable()
    {
        toggleMainAction.performed -= HandleToggleMain;
        togglePauseAction.performed -= HandleTogglePause;
    }

    private void HandleToggleMain(InputAction.CallbackContext context)
    {
        if (!isMainOpen)
        {
            main.AnimateOpen(PanelAnimator.PanelPosition.RIGHT, PanelAnimator.PanelAnimationSpeed.NORMAL);
            mainBackground.AnimateOpen(PanelAnimator.PanelPosition.CENTER, PanelAnimator.PanelAnimationSpeed.NORMAL);
        }
        else
        {
            main.AnimateClose(PanelAnimator.PanelPosition.RIGHT, PanelAnimator.PanelAnimationSpeed.NORMAL);
            mainBackground.AnimateClose(PanelAnimator.PanelPosition.CENTER, PanelAnimator.PanelAnimationSpeed.NORMAL);
        }

        isMainOpen = !isMainOpen;
        stateChanged.RaiseEvent(GetUIState());
    }

    private void HandleTogglePause(InputAction.CallbackContext context)
    {
        if (!isPauseOpen)
            pause.InstantOpen();
        else
            pause.InstantClose();

        isPauseOpen = !isPauseOpen;
        stateChanged.RaiseEvent(GetUIState());
    }

    /// <summary>
    /// Get the current state of the UI system
    /// </summary>
    private UIState GetUIState()
    {
        if (isPauseOpen)
            return UIState.PAUSE;           // Notice that pause takes priority
        else if (isMainOpen)
            return UIState.INVENTORY;
        else
            return UIState.CLEAR;
    }
}






