using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

//For loading next scene on interact.
public class ToNextScene : MonoBehaviour, IAction
{
    private bool activated;
    private RawImage blackScreen;
    private FirstPersonController player;
    
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
    }

    // If door is activated, start the coroutine to load next scene.
    public void Activate()
    {
        // Debug.Log("Loading scene");
        StartCoroutine("LoadStuff");
    }

    private IEnumerator LoadStuff()
    {
        // Play sound and freeze time (Silent Hill 2 style)
        Globals.curSpawnPoint = spawnPoint;
        FMODManager.Instance.PlaySound(FMODManager.SFX.door_open);
        blackScreen.DOFade(1f, 0.75f).SetUpdate(true);
        Time.timeScale = 0f;
        player.cameraCanMove = false; // For some reason this is separate from timeScale

        // Load next scene and play door closing sound
        yield return new WaitForSecondsRealtime(1f);
        SceneManager.LoadScene(nextScene);
        FMODManager.Instance.PlaySound(FMODManager.SFX.door_close);
    }
}
