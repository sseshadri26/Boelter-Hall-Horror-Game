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
    [SerializeField] public UIStateEventChannelSO stateChanged;

    [Header("Panels")]
    [SerializeField] PanelUI pausePanel;
    [SerializeField] PanelUI inventoryPanel;
    [SerializeField] PanelUI journalPanel;
    

    // Map from a panel to its associated UIState
    Dictionary<PanelUI, UIState> panelStateLabel = new Dictionary<PanelUI, UIState>();

    // Set of all open panels
    // DESIGN CHOICE: Why not dictionary of bools? Means we loop through fewer panels during
    // the audit of open panels. It's also a simpler data structure that can do the same job.
    HashSet<PanelUI> openPanels = new HashSet<PanelUI>();
    
    // Set of all panels that overlay on top of panels (instead of pushing them out of the way)
    HashSet<PanelUI> overlayPanels = new HashSet<PanelUI>();

    // List of input actions and their associated callbacks
    List<Tuple<InputAction, Action<InputAction.CallbackContext>>> panelToggleInputCallbackInfo;

    // Player input source
    Controls.FirstPersonActions firstPersonActions;



    void Awake()
    {
        firstPersonActions = playerInput.controls;

        // Set up labels for quickly accessing corresponding 
        panelStateLabel[inventoryPanel] = UIState.INVENTORY;
        panelStateLabel[pausePanel] = UIState.PAUSE;
        panelStateLabel[journalPanel] = UIState.JOURNAL;

        overlayPanels.Add(pausePanel);

    }

    void Start()
    {
        // Broadcast the initial state so that listeners can get initialized (in Start since they may subscribe during Awake)
        stateChanged.RaiseEvent(UIState.CLEAR);
    }

    void OnEnable()
    {
        panelToggleInputCallbackInfo = new List<Tuple<InputAction, Action<InputAction.CallbackContext>>>()
        {
            new Tuple<InputAction, Action<InputAction.CallbackContext>>(firstPersonActions.Inventory, (obj) => HandleToggle(inventoryPanel)),
            new Tuple<InputAction, Action<InputAction.CallbackContext>>(firstPersonActions.Journal, (obj) => HandleToggle(journalPanel)),
            new Tuple<InputAction, Action<InputAction.CallbackContext>>(firstPersonActions.Pause, (obj) => HandleToggle(pausePanel))
        };

        RegisterInputCallbacks(panelToggleInputCallbackInfo);
    }

    void OnDisable()
    {
        DeregisterInputCallbacks(panelToggleInputCallbackInfo);
    }


    /// <summary>
    /// Handle input request to toggle on/off a given panel
    /// </summary>
    private void HandleToggle(PanelUI panel)
    {
        if(openPanels.Contains(panel))
            ClosePanel(panel);
        else
        {
            OpenPanel(panel);
        }
        
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
        foreach(PanelUI panel in openPanels)
        {
            currentPanel = panel;
            
            if(overlayPanels.Contains(currentPanel))
                break;
        }

        if(currentPanel == null)
            return UIState.CLEAR;

        return panelStateLabel[currentPanel];
    }

    /// <summary>
    /// Open this panel and remember its new state
    /// </summary>
    private void OpenPanel(PanelUI panel)
    {
        panel.OpenPanel();

        if(!overlayPanels.Contains(panel))
            CloseAllPanels();

        openPanels.Add(panel);
    }

    /// <summary>
    /// Close this panel and remember its new state
    /// </summary>
    private void ClosePanel(PanelUI panel)
    {
        panel.ClosePanel();
        openPanels.Remove(panel);
    }

    /// <summary>
    /// Close all open panels
    /// </summary>
    private void CloseAllPanels()
    {
        // Need to copy list to avoid errors thrown when trying to modify the dictionary while iterating through it
        List<PanelUI> openPanelsCopy = new List<PanelUI>(openPanels);
        foreach(PanelUI openPanel in openPanelsCopy)
        {
            if(!overlayPanels.Contains(openPanel))
                ClosePanel(openPanel);
        }
    }




    /////////////
    // UTILITY //
    /////////////

    private void RegisterInputCallbacks(List<Tuple<InputAction, Action<InputAction.CallbackContext>>> callbackInfoItems)
    {
        foreach(Tuple<InputAction, Action<InputAction.CallbackContext>> infoItem in callbackInfoItems)
        {
            infoItem.Item1.performed += infoItem.Item2;
        }
    }

    private void DeregisterInputCallbacks(List<Tuple<InputAction, Action<InputAction.CallbackContext>>> callbackInfoItems)
    {
        foreach(Tuple<InputAction, Action<InputAction.CallbackContext>> infoItem in callbackInfoItems)
        {
            infoItem.Item1.performed -= infoItem.Item2;
        }
    }
}






