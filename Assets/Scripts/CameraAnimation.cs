using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class CameraAnimation : MonoBehaviour
{
    //public GameObject playerCamera;

    //// Update is called once per frame

    //private void Start()
    //{
    //    playerCamera.SetActive(true);
    //    if (playerCamera.activeSelf)
    //    {
    //        UnityEngine.Debug.Log("player cam active");
    //    }
    //}
    Camera mainCam;
    Camera secondCamera;
    bool switched = false;

    void Start()
    {
        mainCam = Camera.main;
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


        // if trigger is entered and camera is not switched run function SwitchToSecondCam()

        if (Input.GetKeyDown(KeyCode.Space) && !switched)
        {
            SwitchToSecondCam();
        }
    }

    void SwitchToSecondCam()
    {     
        secondCamera.CopyFrom(mainCam);
        //secondCamera.SetReplacementShader(replacementShader, null);
        mainCam.enabled = false;
        switched = true;
    }


}
