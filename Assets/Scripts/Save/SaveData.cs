using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class SaveData
{
    // We're using arrays because they're serializable
    public float[] playerPos;
    public string playerScene;

    // Create a SaveData from the static PlayerData class
    public void CopyFromGame()
    {
        playerPos = new float[3];

        playerScene = SceneManager.GetActiveScene().name;
    }
}