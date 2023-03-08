using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIController : MonoBehaviour
{
    // DESIGN CHOICE: Base input off of generic input asset to make it
    // a lot simpler to swap in the game's input asset. Ideally, we would have
    // a centralized source of input (just one input ScriptableObject that has all
    // the input callbacks in the game), but because we did not establish any
    // kind of standard for how input should be passed around, I will use the
    // most generic option, which is to take a generic input asset and search for
    // the actions by name, which is more prone to error since strings are prone
    // to error.

    [SerializeField] InputActionAsset input;

    [SerializeField] InventoryUI inventoryPanel;
    [SerializeField] PauseUI pausePanel;


    // ACTION NAMES
    const string a_PanelManagement = "PanelManagement";
    const string a_Inventory = "Inventory";
    const string a_Pause = "Pause";

    // DESIGN CHOICE: Use dictionary as a simple way to keep track of the open/close
    // state of every panel. Dictionaries are more scalable than making a new bool
    // every time we want to keep track of a new panel. 
    Dictionary<PanelUI, bool> panelOpenState = new Dictionary<PanelUI, bool>();

    private InputActionMap panelManagementMap;
    void Awake()
    {
        panelManagementMap = input.FindActionMap(a_PanelManagement);

        // Ensure all panels are closed
        panelOpenState[inventoryPanel] = false;
        panelOpenState[pausePanel] = false;

        foreach(PanelUI panel in panelOpenState.Keys)
        {
            panel.ClosePanel();
        }

        panelManagementMap.Enable();
    }

    void OnEnable()
    {
        panelManagementMap.FindAction(a_Inventory).performed += HandleToggleInventory;
        panelManagementMap.FindAction(a_Pause).performed += HandleTogglePause;
    }

    void OnDisable()
    {
        panelManagementMap.FindAction(a_Inventory).performed -= HandleToggleInventory;
        panelManagementMap.FindAction(a_Pause).performed -= HandleTogglePause;
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
    }
    private void ClosePanel(PanelUI panel)
    {
        panel.ClosePanel();
        panelOpenState[panel] = false;
    }
}
