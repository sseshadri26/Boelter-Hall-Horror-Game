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
    //public Camera secondCamera;
    //bool switched = false;
    //PlayableDirector director;

    void Start()
    {
        //director = GetComponent<PlayableDirector>();
    }

    void Update()
    {
        // if camera is switched, output to the debug log that the camera is switched
        //if (!switched)
        //{
            //UnityEngine.Debug.Log("Camera not switched");
        //}
        //if (switched)
        //{
            //UnityEngine.Debug.Log("Camera switched");
            //director.Play();
        //}       
    }
    [SerializeField] private GameObject _cutscene;
    private void OnTriggerEnter(Collider other)
    {
        //if (!switched)
        //{
        //SwitchToSecondCam();
        //}
        UnityEngine.Debug.Log("Player entered trigger");
        if (other.tag == "Player")
        {
            _cutscene.SetActive(true);
            
        }
    }
    //void SwitchToSecondCam()
    //{     
        //secondCamera.CopyFrom(Camera.main);
        
        //switched = true;
    //}
}
