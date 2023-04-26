using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMODUnity;

//For loading next scene on interact.
public class ToNextScene : MonoBehaviour, IAction
{
    private bool activated;
    
    [Tooltip("The next scene to load")]
    public string nextScene;
    [Tooltip("Which spawn point the player will start at in the next scene")]
    public int spawnPoint;

    void Awake()
    {
        if (SceneManager.GetSceneByName(nextScene) == null)
        {
            Debug.LogError("There is no scene with the name \"" + nextScene + "\". Make sure that it's listed in Build Settings!");
        }
    }

    // If door is activated, start the coroutine to load next scene.
    public void Activate()
    {
        // Debug.Log("Loading scene");
        Globals.curSpawnPoint = spawnPoint;
        StartCoroutine("LoadStuff");
    }

    private IEnumerator LoadStuff()
    {
        FMODManager.Instance.PlaySound(FMODManager.SFX.door_open);
        Scene oldScene = SceneManager.GetActiveScene();

        yield return new WaitForSecondsRealtime(1f);
        SceneManager.LoadScene(nextScene);
    }
}
