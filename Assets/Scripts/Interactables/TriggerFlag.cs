using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

//For loading next scene on interact.
public class TriggerFlag : MonoBehaviour, IAction
{
    [Tooltip("The flag to change")]
    public string flagName;
    [Tooltip("What to change this flag to")]
    public bool flagSetting = true;
    [Tooltip("Whether interacting requires a key (or lockpick)")]
    public bool requiresKey = false;
    [Tooltip("If this Interactable needs a key, the key's name")]
    public string keyName;

    // If door is activated, start the coroutine to load next scene.
    public void Activate()
    {
        // Make sure we don't need a key first
        if (requiresKey && !YarnFunctions.HasItem(keyName))
        {
            return;
        }
        
        Globals.flags[flagName] = requiresKey;
        Debug.Log("Set " + flagName + " to " + Globals.flags[flagName]);
    }
}
