using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSwitch : MonoBehaviour , IAction
{
    // Testing function to see if saves work
    public void Start()
    {
        SaveData data = SaveSystem.LoadGame();
        Debug.Log("Player starts in " + ((data != null) ? data.playerScene : "???"));
    }

    public void Activate()
    {
        Debug.Log("Saving!");
        SaveSystem.SaveGame();
    }
}
