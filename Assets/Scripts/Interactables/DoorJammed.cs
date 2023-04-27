using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//For loading next scene on interact.
public class DoorJammed : MonoBehaviour, IAction
{
    private bool activated;
    private FirstPersonController player;

    private void Start()
    {
        player = GameObject.FindObjectOfType<FirstPersonController>();
    }

    // If door is activated, load next scene.
    public void Activate()
    {
        Notification.instance.ShowMessage("The door won't open");
        StartCoroutine("FreezePlayer");
    }

    private IEnumerator FreezePlayer()
    {
        player.playerCanMove = false;
        player.cameraCanMove = false;

        yield return new WaitForSeconds(1.5f);
        player.playerCanMove = true;
        player.cameraCanMove = true;
    }
}
