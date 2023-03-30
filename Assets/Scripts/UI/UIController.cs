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

    [Header("Panels")]
    [SerializeField] PanelUI pausePanel;
    [SerializeField] PanelUI inventoryPanel;
    [SerializeField] PanelUI journalPanel;
    


    // DESIGN CHOICE: Use dictionary as a simple way to keep track of the open/close
    // state of every panel. Dictionaries are more scalable than making a new bool
    // every time we want to keep track of a new panel. 
    Dictionary<PanelUI, bool> panelOpenState = new Dictionary<PanelUI, bool>();

    Dictionary<PanelUI, UIState> panelStateLabel = new Dictionary<PanelUI, UIState>();
    
    // Set of all panels that overlay on top of all
    HashSet<PanelUI> overlayPanels = new HashSet<PanelUI>();

    private Controls.FirstPersonActions firstPersonActions;

    void Awake()
    {
        firstPersonActions = playerInput.controls;

        // Ensure all panels are closed
        panelOpenState[inventoryPanel] = false;
        panelOpenState[pausePanel] = false;
        panelOpenState[journalPanel] = false;

        // Set up labels for quickly accessing corresponding 
        panelStateLabel[inventoryPanel] = UIState.INVENTORY;
        panelStateLabel[pausePanel] = UIState.PAUSE;
        panelStateLabel[journalPanel] = UIState.JOURNAL;

        overlayPanels.Add(pausePanel);

    }

    // TODO:
    // Is there any cleaner way to register/deregister? If we use an anonymous function we could save
    // lots of code repeat, but we would be unable to deregister?
    // TODO: 
    // Make use of overlay set to figure out which panels should kick others out
    void OnEnable()
    {
        firstPersonActions.Inventory.performed += HandleToggleInventory;
        firstPersonActions.Pause.performed += HandleTogglePause;
        firstPersonActions.Journal.performed += HandleToggleJournal;
    }

    void OnDisable()
    {
        firstPersonActions.Inventory.performed -= HandleToggleInventory;
        firstPersonActions.Pause.performed -= HandleTogglePause;
        firstPersonActions.Journal.performed -= HandleToggleJournal;
    }

    private void HandleToggleJournal(InputAction.CallbackContext obj)
    {
        if(panelOpenState[journalPanel])
            ClosePanel(journalPanel);
        else
            OpenSoloPanel(journalPanel);

        stateChanged.RaiseEvent(GetUIState());
    }

    private void HandleTogglePause(InputAction.CallbackContext obj)
    {
        if(panelOpenState[pausePanel])
            ClosePanel(pausePanel);
        else
            // Notice that the pause panel is special in that it covers up others instead of pushing them away
            OpenPanel(pausePanel);
        
        stateChanged.RaiseEvent(GetUIState());
    }

    private void HandleToggleInventory(InputAction.CallbackContext obj)
    {
        if(panelOpenState[inventoryPanel])
            ClosePanel(inventoryPanel);
        else
            OpenSoloPanel(inventoryPanel);
        
        stateChanged.RaiseEvent(GetUIState());
    }


    /// <summary>
    /// Get the current state of the UI system
    /// </summary>
    private UIState GetUIState()
    {
        // DESIGN CHOICE: Use a dedicated "auditing" function that examines the
        // state of the UI instead of keeping track of open panels as they're
        // opened and closed since it offers a more straightforward, reliable
        // solution for figuring out what the state should be. Here, "reliable" means
        // it is pretty much guaranteed to work even as more states are added.
        // It may be slow to do this audit, but that's okay because there probably
        // won't be a ton of panels, and this probably won't be called a lot.

        PanelUI currentPanel = null;
        foreach(var panelToIsOpenPair in panelOpenState)
        {
            if(panelToIsOpenPair.Value)
            {
                if(panelToIsOpenPair.Key == pausePanel)
                    return UIState.PAUSE;
                else
                    currentPanel = panelToIsOpenPair.Key;
            }
        }

        if(currentPanel == null)
            return UIState.CLEAR;
        else
            return panelStateLabel[currentPanel];
    }

    /// <summary>
    /// Open this panel and remember its new state
    /// </summary>
    private void OpenPanel(PanelUI panel)
    {
        panel.OpenPanel();
        panelOpenState[panel] = true;
    }

    /// <summary>
    /// Open this panel and close all others and remember its new state
    /// </summary>
    private void OpenSoloPanel(PanelUI panel)
    {
        OpenPanel(panel);

        // Need to copy list to avoid errors thrown when trying to modify the dictionary while iterating through it
        List<PanelUI> others = new List<PanelUI>(panelOpenState.Keys);
        foreach(PanelUI other in others)
        {
            if(other != panel && !overlayPanels.Contains(other))
                ClosePanel(other);
        }
    }

    /// <summary>
    /// Close this panel and remember its new state
    /// </summary>
    private void ClosePanel(PanelUI panel)
    {
        panel.ClosePanel();
        panelOpenState[panel] = false;
    }
}
