using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//For loading next scene on interact.
public class ToNextScene : MonoBehaviour, IAction
{
    private bool activated;
    
    [Tooltip("The next scene to load")]
    public string nextScene;
    [Tooltip("Which spawn point the player will start at in the next scene")]
    public int spawnPoint;

    // If door is activated, load next scene.
    public void Activate()
    {
        Globals.curSpawnPoint = spawnPoint;
        SceneManager.LoadScene(nextScene);
    }
}
