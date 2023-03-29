using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is used for relaying information about the state of the UI
/// Example: An Achievement unlock event, where the int is the Achievement ID.
/// </summary>
[CreateAssetMenu(menuName = "ScriptableObjects/Events/UI State Event Channel")]
public class UIStateEventChannelSO : EventChannelSO<UIState>
{}

public enum UIState
{
    CLEAR,
    INVENTORY,
    JOURNAL,
    PAUSE
}