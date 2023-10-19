using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Yarn.Unity;

//For loading next scene on interact.
[RequireComponent(typeof(Animator))]
public class Carey : MonoBehaviour, IAction
{
    public GameObject cup;

    private bool activated;
    private FirstPersonController fpc;
    private Animator anim;

    private void Start()
    {
        fpc = FirstPersonController.instance;
        anim = GetComponent<Animator>();
    }

    // If Carey is activated, talk to him.
    public void Activate()
    {
        Globals.flags.Add("Cafe", true);
        fpc.controls.Disable();
        fpc.dialogueRunner.StartDialogue("Cafe");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        StartCoroutine("AllowContinueInput");

        transform.DOLookAt(fpc.transform.position, 2f, AxisConstraint.Y);
    }

    private IEnumerator AllowContinueInput()
    {
        yield return new WaitForSeconds(0.5f);
        FindObjectOfType<DialogueAdvanceInput>().Action.Enable();
    }

    [YarnCommand("trigger")]
    public void Trigger(string animationTrigger)
    {
        anim.SetTrigger(animationTrigger);

        if (animationTrigger == "Drink")
        {
            StartCoroutine("Drink");
        }
    }

    private IEnumerator Drink()
    {
        yield return new WaitForSeconds(1f);
        cup.SetActive(true);

        yield return new WaitForSeconds(4f);
        cup.SetActive(false);
    }
}
