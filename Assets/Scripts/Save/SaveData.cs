using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class SaveData
{
    // We're using arrays because they're serializable
    public float[] playerPos;
    public float[] playerRot;
    public string playerScene;

    // Create a SaveData from the static PlayerData class
    public void CopyFromGame()
    {
        Transform player = FirstPersonController.instance.transform;
        
        playerPos = new float[3];
        playerPos[0] = player.position.x;
        playerPos[1] = player.position.y;
        playerPos[2] = player.position.z;
        
        playerRot = new float[4];
        playerRot[0] = player.rotation.x;
        playerRot[1] = player.rotation.y;
        playerRot[2] = player.rotation.z;
        playerRot[3] = player.rotation.w;

        playerScene = SceneManager.GetActiveScene().name;
    }
}