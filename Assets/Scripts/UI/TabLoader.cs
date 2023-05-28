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


    void Awake()
    {
        loadMapEvent.OnEventRaised += HandleLoadMapEvent;
    }

    private void HandleLoadMapEvent()
    {
        tabPanelUI.AddNewTab(mapTabData);
    }
}
