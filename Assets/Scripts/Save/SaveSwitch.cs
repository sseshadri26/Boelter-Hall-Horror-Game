using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSwitch : MonoBehaviour, IAction
{
    // Testing function to see if saves work
    public void Start()
    {
        SaveData data = SaveSystem.LoadGame();
        if (data == null) return;
        Transform player = FirstPersonController.instance.transform;

        Vector3 position = new Vector3(data.playerPos[0], data.playerPos[1], data.playerPos[2]);
        Quaternion rotation = new Quaternion(data.playerRot[0], data.playerRot[1], data.playerRot[2], data.playerRot[3]);

        Debug.Log("Player starts in " + ((data != null) ? data.playerScene : "???"));
    }

    public void Activate()
    {
        Debug.Log("Saving!");
        SaveSystem.SaveGame();
    }
}
