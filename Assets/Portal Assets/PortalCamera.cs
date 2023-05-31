using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCamera : MonoBehaviour
{

    public Camera playerCamera;
    public Transform portal;
    public Transform otherPortal;
    public bool visible;

    [SerializeField] private PortalVisibility portalVisibility;
    [SerializeField] private Camera portalCamera;

    [SerializeField] private MeshRenderer meshRenderer;
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
        // portalCamera = GetComponent<Camera>();
        //if not on screen, don't render


    }

    //Update is called once per frame
    void LateUpdate()
    {


        Vector3 playerOffsetFromPortal = Quaternion.Inverse(otherPortal.rotation) * (playerCamera.transform.position - otherPortal.position);

        transform.localPosition = playerOffsetFromPortal;


        transform.localRotation = Quaternion.Inverse(otherPortal.rotation) * playerCamera.transform.rotation;

        if (meshRenderer != null)
        {
            if (!portalVisibility.isVisible && !portalVisibility.closeToPortal)
            {
                visible = false;
                portalCamera.enabled = false;
            }
            else
            {
                visible = true;
                portalCamera.enabled = true;
            }
        }


    }



}
