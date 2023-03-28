using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//For loading next scene on interact.
public class TV : MonoBehaviour, IAction
{
    private bool activated;
    private FirstPersonController fpc;

    private void Start()
    {
        fpc = FirstPersonController.instance;
    }

    // If door is activated, load next scene.
    public void Activate()
    {
        fpc.controls.Disable();
        fpc.dialogueRunner.StartDialogue("Intro");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
