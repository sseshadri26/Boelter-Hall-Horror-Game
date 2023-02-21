using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerConfines : MonoBehaviour
{
    void Awake()
    {
        // When first loading the scene, move the player to the current start position
        Transform spawnPoint = GameObject.FindGameObjectWithTag("spawnPointsParent").transform.GetChild(Globals.curSpawnPoint);
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;
    }
}
