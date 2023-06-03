using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

//For loading next scene on interact.
[RequireComponent(typeof(Animator))]
public class ElevatorCutscene : MonoBehaviour, IAction
{
    private bool activated;
    private RawImage blackScreen;
    private FirstPersonController player;
    private Animator anim;

    [Tooltip("The next scene to load")]
    public string nextScene;
    [Tooltip("Which spawn point the player will start at in the next scene")]
    public int spawnPoint;

    void Start()
    {
        if (SceneManager.GetSceneByName(nextScene) == null)
        {
            Debug.LogError("There is no scene with the name \"" + nextScene + "\". Make sure that it's listed in Build Settings!");
        }
        blackScreen = GameObject.FindGameObjectWithTag("BlackScreen").GetComponent<RawImage>();
        player = GameObject.FindObjectOfType<FirstPersonController>();
        anim = GetComponent<Animator>();
    }

    // If door is activated, start the coroutine to load next scene.
    public void Activate()
    {
        // Make sure we don't need a key first
        StartCoroutine("Cutscene");
    }

    private IEnumerator Cutscene()
    {
        anim.SetTrigger("Close");
        yield return new WaitForSeconds(2f);

        // Play sound and freeze time (Silent Hill 2 style)
        Globals.curSpawnPoint = spawnPoint;
        // FMODManager.Instance.PlaySound(FMODManager.SFX.door_open);
        blackScreen.DOFade(1f, 0.75f).SetUpdate(true);
        // Time.timeScale = 0f;

        // Load next scene and play door closing sound
        yield return new WaitForSecondsRealtime(1f);
        Globals.playDoorCloseSoundAtNextScene = false;
        SceneManager.LoadScene(nextScene);
    }

    private IEnumerator FreezePlayer()
    {
        player.playerCanMove = false;

        yield return new WaitForSeconds(1.5f);
        player.playerCanMove = true;
    }
}
