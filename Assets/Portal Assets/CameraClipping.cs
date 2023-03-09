using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraClipping : MonoBehaviour
{

    public Camera playerCamera;
    public Camera portalCamera;

    public Matrix4x4 projectionMatrix;

    [Header("Advanced Settings")]
    public float nearClipOffset = 0.05f;
    public float nearClipLimit = 0.2f;


    // Start is called before the first frame update
    void Start()
    {
        //if playerCamera is not set in the editor, set it to a default Camera.main
        if (playerCamera == null)
        {
            // print to log 
            //Debug.Log("Player camera not set in editor, setting to Camera.main");
            playerCamera = Camera.main;
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        SetNearClipPlane();
    }

    void SetNearClipPlane()
    {


        // Learning resource:
        // http://www.terathon.com/lengyel/Lengyel-Oblique.pdf
        Transform clipPlane = transform;
        int dot = System.Math.Sign(Vector3.Dot(clipPlane.forward, transform.position - portalCamera.transform.position));

        Vector3 camSpacePos = portalCamera.worldToCameraMatrix.MultiplyPoint(clipPlane.position);
        Vector3 camSpaceNormal = portalCamera.worldToCameraMatrix.MultiplyVector(clipPlane.forward) * dot;
        float camSpaceDst = -Vector3.Dot(camSpacePos, camSpaceNormal);

        //// Don't use oblique clip plane if very close to portal as it seems this can cause some visual artifacts
        //if (Mathf.Abs(camSpaceDst) > nearClipLimit)
        //{



            //Vector4 clipPlaneCameraSpace = new Vector4(camSpaceNormal.x, camSpaceNormal.y, camSpaceNormal.z, camSpaceDst);

            //Vector4 clipPlaneCameraSpace = new Vector4(camSpaceNormal.x, camSpaceNormal.y, camSpaceNormal.z, camSpaceDst);



            ////keeep!!!
            ///

            //Vector4 clipPlaneCameraSpace = new Vector4(camSpaceNormal.x, camSpaceNormal.y, camSpaceNormal.z, camSpaceDst);

            //projectionMatrix = playerCamera.CalculateObliqueMatrix(clipPlaneCameraSpace);
            //portalCamera.projectionMatrix = projectionMatrix;



            /////keep!
            ///

            // Update projection based on new clip plane
            // Calculate matrix with player cam so that player camera settings (fov, etc) are used
            //try catch this
            //try
            //{
            //    projectionMatrix = playerCamera.CalculateObliqueMatrix(clipPlaneCameraSpace);
            //    Debug.Log(projectionMatrix);
            //    portalCamera.projectionMatrix = projectionMatrix;
            //}

            //catch
            //{
            //    Debug.Log(projectionMatrix);
            //    portalCamera.projectionMatrix = playerCamera.projectionMatrix;
            //}

        //}
        //else
        //{
            //projectionMatrix = playerCamera.projectionMatrix;
            //portalCamera.projectionMatrix = playerCamera.projectionMatrix;
        //}

        //float d = 10;
        //if (camSpaceDst < d && camSpaceDst > 0)
        //{
        //    camSpaceDst = d;
        //}
        //if (camSpaceDst > -d && camSpaceDst < 0)
        //{
        //    camSpaceDst = -d;
        //}


        //Vector4 clipPlaneCameraSpace = new Vector4(camSpaceNormal.x, camSpaceNormal.y, camSpaceNormal.z, camSpaceDst);
        //portalCamera.projectionMatrix = playerCamera.CalculateObliqueMatrix(clipPlaneCameraSpace);

        // Update projection based on new clip plane
        // Calculate matrix with player cam so that player camera settings (fov, etc) are used



    }
}
