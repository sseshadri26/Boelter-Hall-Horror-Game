using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class CameraAnimation : MonoBehaviour
{
    public GameObject playerCamera;
    public GameObject jumpCamera;

    // Update is called once per frame

    private void Start()
    {
        playerCamera.SetActive(true);
        jumpCamera.SetActive(false);
        if (playerCamera.activeSelf)
        {
            UnityEngine.Debug.Log("player cam active");
        }
    }

    // if player enters trigger, set jump cam active and player cam inactive
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerCamera.SetActive(false);
            jumpCamera.SetActive(true);
            UnityEngine.Debug.Log("jump cam active");
        }
    }
}
