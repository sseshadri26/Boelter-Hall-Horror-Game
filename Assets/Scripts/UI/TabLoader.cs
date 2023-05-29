using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script for adding in a new tab to the tab panel during the game.
/// </summary>
public class TabLoader : MonoBehaviour
{
    [SerializeField] TabPanelUI tabPanelUI;
    [SerializeField] VoidEventChannelSO loadMapEvent;
    [SerializeField] TabPanelUI.TabPanelData mapTabData;


    void Start()
    {
        // If the map has been unlocked before this scene was loaded
        // Defer until start to give time for tab panel to be initialized
        if (Globals.flags.ContainsKey("Map") && Globals.flags["Map"])
        {
            tabPanelUI.AddNewTab(mapTabData);
        }

    }
    void OnEnable()
    {
        loadMapEvent.OnEventRaised += HandleLoadMapEvent;
    }

    void OnDisable()
    {
        loadMapEvent.OnEventRaised -= HandleLoadMapEvent;
    }


    // Handler for unlocking map after UI scene has been loaded
    private void HandleLoadMapEvent()
    {
        // We trust that this won't be called if map has already
        // been added (which would lead to duplicates)
        tabPanelUI.AddNewTab(mapTabData);
    }
}
