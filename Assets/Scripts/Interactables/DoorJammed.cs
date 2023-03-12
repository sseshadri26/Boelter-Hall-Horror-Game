using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//For loading next scene on interact.
public class DoorJammed : MonoBehaviour, IAction
{
    private bool activated;

    // If door is activated, load next scene.
    public void Activate()
    {
        Debug.Log("The door is jammed!");
    }
}
