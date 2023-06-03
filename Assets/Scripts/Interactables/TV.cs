using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn.Unity;

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
        fpc.dialogueRunner.StartDialogue("Floor5");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(true);
        fpc.dialogueRunner.onDialogueComplete.AddListener(() =>
        {
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(false);
        });
        StartCoroutine("AllowContinueInput");
    }

    public void ActivateIntro()
    {
        fpc.controls.Disable();
        fpc.dialogueRunner.StartDialogue("Intro");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(true);
        fpc.dialogueRunner.onDialogueComplete.AddListener(() =>
        {
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(false);
        });
        StartCoroutine("AllowContinueInput");
    }

    private IEnumerator AllowContinueInput()
    {
        yield return new WaitForSeconds(0.5f);
        FindObjectOfType<DialogueAdvanceInput>().Action.Enable();
    }
}
