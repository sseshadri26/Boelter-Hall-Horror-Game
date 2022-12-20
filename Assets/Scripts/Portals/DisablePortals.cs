using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisablePortals : MonoBehaviour
{

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

    [SerializeReference]
    bool screensAreEnabled;



    // Start is called before the first frame update
    void Start()
    {
        screensAreOnScreen = Portal1Visibility.isVisible && Portal2Visibility.isVisible;
        previousScreensAreOnScreen = screensAreOnScreen;
        inFrontOfScreens = Portal1Visibility.playerIsInFrontOfPortal && Portal2Visibility.playerIsInFrontOfPortal;
        Portal1Material = GetComponent<PortalTextureSetup>().cameraMatA;
        Portal2Material = GetComponent<PortalTextureSetup>().cameraMatB;

    }

    // Update is called once per frame
    void Update()
    {
        screen1IsOnScreen = Portal1Visibility.isVisible;
        screen2IsOnScreen = Portal2Visibility.isVisible;

        inFrontOfScreens = Portal1Visibility.playerIsInFrontOfPortal && Portal2Visibility.playerIsInFrontOfPortal;

        screensAreOnScreen = Portal1Visibility.isVisible || Portal2Visibility.isVisible;

        portalsColliding = Portal1Teleporter.playerIsOverlapping || Portal2Teleporter.playerIsOverlapping;


        if (screensAreOnScreen == false && previousScreensAreOnScreen == true && !portalsColliding)
        {
            screensAreEnabled = !screensAreEnabled;
            //randomly enable screens with frequency 0.5
            //screensAreEnabled = Random.value < 0.5;


        }
        previousScreensAreOnScreen = screensAreOnScreen;

        if (!inFrontOfScreens && !portalsColliding)
        {
            screensAreEnabled = false;
        }


        if (screensAreEnabled)
        {

            Portal1Screen.material = Portal2Material;
            Portal2Screen.material = Portal1Material;

            Portal1Teleporter.teleportingEnabled = true;
            Portal2Teleporter.teleportingEnabled = true;
        }
        else
        {
            Portal1Screen.material = TransparentMaterial;
            Portal2Screen.material = TransparentMaterial;
            Portal1Teleporter.teleportingEnabled = false;
            Portal2Teleporter.teleportingEnabled = false;



        }

    }


}
