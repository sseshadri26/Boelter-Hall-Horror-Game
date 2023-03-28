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

    [SerializeField] AlmanacUI inventoryPanel;
    [SerializeField] PauseUI pausePanel;


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

        foreach(PanelUI panel in panelOpenState.Keys)
        {
            panel.ClosePanel();
        }
    }

    void OnEnable()
    {
        firstPersonActions.Inventory.performed += HandleToggleInventory;
        firstPersonActions.Pause.performed += HandleTogglePause;
    }

    void OnDisable()
    {
        firstPersonActions.Inventory.performed -= HandleToggleInventory;
        firstPersonActions.Pause.performed -= HandleTogglePause;
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
