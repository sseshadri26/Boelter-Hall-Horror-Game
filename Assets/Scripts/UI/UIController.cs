using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIController : MonoBehaviour
{
    // DESIGN CHOICE: Inject persistent input asset into UI controls so that it does not
    // rely on runtime reference to input (preventing it from running properly without its owner being instantiated)
    [SerializeField] FirstPersonActionsSO firstPersonActionsSO;

    [SerializeField] PanelUI inventoryPanel;
    [SerializeField] PanelUI pausePanel;
    [SerializeField] PanelUI journalPanel;


    // DESIGN CHOICE: Use dictionary as a simple way to keep track of the open/close
    // state of every panel. Dictionaries are more scalable than making a new bool
    // every time we want to keep track of a new panel. 
    Dictionary<PanelUI, bool> panelOpenState = new Dictionary<PanelUI, bool>();

    private Controls.FirstPersonActions firstPersonActions;
    void Awake()
    {
        firstPersonActions = firstPersonActionsSO.controls;

        // Ensure all panels are closed
        panelOpenState[inventoryPanel] = false;
        panelOpenState[pausePanel] = false;
        panelOpenState[journalPanel] = false;

    }

    // TODO:
    // Is there any cleaner way to register/deregister? If we use an anonymous function we could save
    // lots of code repeat, but we would be unable to deregister?
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
    }

    private void HandleTogglePause(InputAction.CallbackContext obj)
    {
        if(panelOpenState[pausePanel])
            ClosePanel(pausePanel);
        else
            // Notice that the pause panel is special in that it covers up others instead of pushing them away
            OpenPanel(pausePanel);
    }

    private void HandleToggleInventory(InputAction.CallbackContext obj)
    {
        if(panelOpenState[inventoryPanel])
            ClosePanel(inventoryPanel);
        else
            OpenSoloPanel(inventoryPanel);
    }


    private void OpenPanel(PanelUI panel)
    {
        panel.OpenPanel();
        panelOpenState[panel] = true;
    }

    /// <summary>
    /// Open this panel and close all others
    /// </summary>
    private void OpenSoloPanel(PanelUI panel)
    {
        OpenPanel(panel);

        // Need to copy list to avoid errors thrown when trying to modify the dictionary while iterating through it
        List<PanelUI> others = new List<PanelUI>(panelOpenState.Keys);
        foreach(PanelUI other in others)
        {
            if(other != panel)
                ClosePanel(other);
        }
    }
    private void ClosePanel(PanelUI panel)
    {
        panel.ClosePanel();
        panelOpenState[panel] = false;
    }
}
