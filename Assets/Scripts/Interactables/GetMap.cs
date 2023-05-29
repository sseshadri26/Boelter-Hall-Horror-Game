using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

//For loading next scene on interact.
public class GetMap : MonoBehaviour, IAction
{
    public VoidEventChannelSO MapEvent;

    public void Start()
    {
        if (YarnFunctions.FlagIsTrue("Map"))
        {
            Destroy(this.gameObject);
        }
    }

    // If door is activated, start the coroutine to load next scene.
    public void Activate()
    {
        if (MapEvent != null)
        {
            MapEvent.RaiseEvent();
        }

        Globals.flags["Map"] = true;
        Debug.Log("Set " + "Map" + " to " + Globals.flags["Map"]);

        Destroy(this.gameObject);
    }
}
