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
}
