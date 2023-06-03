using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSwitch : MonoBehaviour, IAction
{
    public int spawnPoint;

    public void Activate()
    {
        Globals.curSpawnPoint = spawnPoint;
        SaveSystem.SaveGame();
        Notification.instance.ShowMessage("Saved");
    }
}
