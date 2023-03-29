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

    Dictionary<PanelUI, List<PanelUI>> mutualExclusion = new Dictionary<PanelUI, List<PanelUI>>();

    private Controls.FirstPersonActions firstPersonActions;
    void Awake()
    {
        firstPersonActions = firstPersonActionsSO.controls;

        // Ensure all panels are closed
        panelOpenState[inventoryPanel] = false;
        panelOpenState[pausePanel] = false;
        panelOpenState[journalPanel] = false;

        // Set up mutually exclusive panels
        mutualExclusion[inventoryPanel] = new List<PanelUI>() {journalPanel};
        mutualExclusion[pausePanel] = new List<PanelUI>() {};
        mutualExclusion[journalPanel] = new List<PanelUI>() {inventoryPanel};

    }

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
            OpenPanel(journalPanel);
    }

    private void HandleTogglePause(InputAction.CallbackContext obj)
    {
        if(panelOpenState[pausePanel])
            ClosePanel(pausePanel);
        else
            OpenPanel(pausePanel);
    }

    private void HandleToggleInventory(InputAction.CallbackContext obj)
    {
        if(panelOpenState[inventoryPanel])
            ClosePanel(inventoryPanel);
        else
            OpenPanel(inventoryPanel);
    }

    private void OpenPanel(PanelUI panel)
    {
        panel.OpenPanel();
        panelOpenState[panel] = true;

        // Close all other panels that aren't allowed to be open at the same time
        foreach(PanelUI other in mutualExclusion[panel])
        {
            ClosePanel(other);
        }
    }
    private void ClosePanel(PanelUI panel)
    {
        panel.ClosePanel();
        panelOpenState[panel] = false;
    }
}
