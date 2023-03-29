using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Playables;

public class CameraAnimation : MonoBehaviour
{
    [SerializeField] 
    private GameObject _cutscene;
    public Camera secondCam;
    bool sceneStarted = false;

    private void OnTriggerEnter(Collider other)
    {
        UnityEngine.Debug.Log("Player entered trigger");
        if (sceneStarted == false)
        {
            // set secondCam active and current camera to not active 
            secondCam.gameObject.SetActive(true);
            _cutscene.SetActive(true);
            sceneStarted = true;
        }

        //if (other.CompareTag("Player"))
        //{
        //    _cutscene.SetActive(true);
        //}
    }

}
