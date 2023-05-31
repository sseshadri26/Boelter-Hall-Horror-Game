using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisablePortals : MonoBehaviour
{

    //create two arrays of gameobjects - one set of objects to enable and one to disable as soon as we dissapear this portal

    [SerializeField]
    public GameObject[] objectsToEnable;
    [SerializeField]
    public GameObject[] objectsToDisable;


    [SerializeReference]
    PortalVisibility Portal1Visibility;
    [SerializeReference]
    PortalVisibility Portal2Visibility;


    [SerializeReference]
    MeshRenderer Portal1Screen;
    [SerializeReference]
    MeshRenderer Portal2Screen;

    [SerializeReference]
    Collider Portal1Collider;
    [SerializeReference]
    Collider Portal2Collider;

    [SerializeReference]
    PortalTeleporter Portal1Teleporter;
    [SerializeReference]
    PortalTeleporter Portal2Teleporter;


    Material Portal1Material;
    Material Portal2Material;
    [SerializeReference]
    Material TransparentMaterial;

    public bool screensAreOnScreen;
    public bool screen1IsOnScreen;
    public bool screen2IsOnScreen;

    public bool previousScreensAreOnScreen;

    public bool inFrontOfScreens;

    public bool portalsColliding;

    public bool portalDisabledForever;

    [SerializeReference]
    bool screensAreEnabled;



    // Start is called before the first frame update
    void Start()
    {
        screensAreOnScreen = Portal1Visibility.isVisible && Portal2Visibility.isVisible;
        previousScreensAreOnScreen = screensAreOnScreen;
        inFrontOfScreens = Portal1Visibility.playerIsInFrontOfPortal && Portal2Visibility.playerIsInFrontOfPortal;
        Portal1Material = this.transform.parent.gameObject.GetComponent<PortalTextureSetup>().cameraMatA;
        Portal2Material = this.transform.parent.gameObject.GetComponent<PortalTextureSetup>().cameraMatB;

    }

    // Update is called once per frame
    void Update()
    {
        if (portalDisabledForever && !screensAreEnabled)
        {
            //objectsToEnable and objectsToDisable are set in the editor
            // call PermanentlyDisable in RotatingPortalDisableManager to disable the portal potentially
            this.gameObject.transform.parent.gameObject.GetComponent<RotatingPortalDisableManager>().PermanentlyDisable();

            //disable this script so it doesn't run again
            gameObject.SetActive(false);
        }

        screen1IsOnScreen = Portal1Visibility.isVisible;
        screen2IsOnScreen = Portal2Visibility.isVisible;

        inFrontOfScreens = Portal1Visibility.playerIsInFrontOfPortal && Portal2Visibility.playerIsInFrontOfPortal;

        screensAreOnScreen = Portal1Visibility.isVisible || Portal2Visibility.isVisible;

        portalsColliding = Portal1Teleporter.playerIsOverlapping || Portal2Teleporter.playerIsOverlapping;


        if (screensAreOnScreen == false && !portalsColliding)
        {
            // screensAreEnabled = !screensAreEnabled;
            screensAreEnabled = false;
            portalDisabledForever = true;
            //randomly enable screens with frequency 0.5
            //screensAreEnabled = Random.value < 0.5;


        }
        // previousScreensAreOnScreen = screensAreOnScreen;

        // if (!inFrontOfScreens && !portalsColliding)
        // {
        //     screensAreEnabled = false;
        //     portalDisabledForever = true;
        // }


        // if (screensAreEnabled)
        // {

        //     Portal1Screen.material = Portal2Material;
        //     Portal2Screen.material = Portal1Material;

        //     Portal1Teleporter.teleportingEnabled = true;
        //     Portal2Teleporter.teleportingEnabled = true;
        // }
        // else
        // {
        //     Portal1Screen.material = TransparentMaterial;
        //     Portal2Screen.material = TransparentMaterial;
        //     Portal1Teleporter.teleportingEnabled = false;
        //     Portal2Teleporter.teleportingEnabled = false;



        // }

    }


}
