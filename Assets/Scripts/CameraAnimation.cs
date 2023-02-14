using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraAnimation : MonoBehaviour
{

    //public Camera mainCam;
    public Camera secondCamera;
    bool switched = false;

    void Start()
    {
        //mainCam = Camera.main;
        
    }

    void Update()
    {
        // if camera is switched, output to the debug log that the camera is switched
        if (!switched)
        {
            UnityEngine.Debug.Log("Camera not switched");
        }
        if (switched)
        {
            UnityEngine.Debug.Log("Camera switched");
        }       


    }
    private void OnTriggerEnter()
    {
        UnityEngine.Debug.Log("Trigger Entered");
        if (!switched)
        {
            SwitchToSecondCam();
        }
    }
    void SwitchToSecondCam()
    {     
        secondCamera.CopyFrom(Camera.main);
        //secondCamera.SetReplacementShader(replacementShader, null);
        
        
        //mainCam.enabled = false;
        switched = true;
    }


}
