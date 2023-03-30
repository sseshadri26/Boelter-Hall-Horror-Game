using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class CameraAnimation : MonoBehaviour
{
    [SerializeField] private GameObject mainCam;
    [SerializeField] private GameObject _cutscene;
    [SerializeField] private GameObject trigger;
    bool scenePlayed = false;
    

    private void OnTriggerEnter(Collider other)
    {
        UnityEngine.Debug.Log("Player entered trigger");
        if (other.tag == "Player" && scenePlayed == false)
        {            
            mainCam.gameObject.SetActive(false);
            _cutscene.SetActive(true);
            scenePlayed = true;
        }
        trigger.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (scenePlayed == true && !_cutscene.transform.GetChild(0).gameObject.activeInHierarchy)
        {
         
            mainCam.transform.position = _cutscene.transform.GetChild(0).transform.position;
            mainCam.gameObject.SetActive(true);
            scenePlayed = false;
            //UnityEngine.Debug.Log(scenePlayed);
        }      
    }
}
