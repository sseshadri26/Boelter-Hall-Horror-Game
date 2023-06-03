using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Yarn.Unity;
using DG.Tweening;

/// <summary> Static Yarn Spinner commands. Commands either return nothing or are coroutines </summary>
public static class YarnCommands
{
    [YarnCommand("setSprite")]
    public static void SetSprite(string spritePath)
    {
        Material newMat = Resources.Load<Material>(spritePath);

        if (!newMat)
        {
            Debug.LogError("The material in Resources/" + spritePath + " couldn't be found!");
            return;
        }

        foreach (GameObject nach in GameObject.FindGameObjectsWithTag("TV Screen"))
        {
            nach.GetComponent<MeshRenderer>().material = newMat;
        }
    }

    [YarnCommand("loadScene")]
    public static IEnumerator LoadScene(string sceneName, int spawnPoint)
    {
        Globals.curSpawnPoint = spawnPoint;
        GameObject.FindGameObjectWithTag("BlackScreen").GetComponent<RawImage>().DOFade(1f, 0.75f).SetUpdate(true);

        // Play sound after fade out
        yield return new WaitForSecondsRealtime(1f);
        FMODManager.Instance.PlaySound(FMODManager.SFX.door_open);

        // Load next scene and play door closing sound
        yield return new WaitForSecondsRealtime(1f);
        Globals.playDoorCloseSoundAtNextScene = true;
        SceneManager.LoadScene(sceneName);
    }
}
