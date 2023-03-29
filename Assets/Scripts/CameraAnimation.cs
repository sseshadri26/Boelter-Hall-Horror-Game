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
    [SerializeField] private GameObject mainCam;
    [SerializeField] private GameObject _cutscene;
    bool scenePlayed = false;

    private void OnTriggerEnter(Collider other)
    {
        UnityEngine.Debug.Log("Player entered trigger");
        if (other.tag == "Player" && scenePlayed == false)
        {
            mainCam.gameObject.SetActive(false);
            _cutscene.SetActive(true);
            
        }
    }

}
