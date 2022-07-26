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
    Material Portal1Material;
    [SerializeReference]
    Material Portal2Material;
    [SerializeReference]
    Material TransparentMaterial;

    public bool screensAreOnScreen;
    public bool screen1IsOnScreen;
    public bool screen2IsOnScreen;

    public bool previousScreensAreOnScreen;

    public bool inFrontOfScreens;

    [SerializeReference]
    bool screensAreEnabled;



    // Start is called before the first frame update
    void Start()
    {
        screensAreOnScreen = Portal1Visibility.isVisible && Portal2Visibility.isVisible;
        previousScreensAreOnScreen = screensAreOnScreen;
        inFrontOfScreens = Portal1Visibility.playerIsInFrontOfPortal && Portal2Visibility.playerIsInFrontOfPortal;
    }

    // Update is called once per frame
    void Update()
    {
        screen1IsOnScreen = Portal1Visibility.isVisible;
        screen2IsOnScreen = Portal2Visibility.isVisible;

        inFrontOfScreens = Portal1Visibility.playerIsInFrontOfPortal && Portal2Visibility.playerIsInFrontOfPortal;

        screensAreOnScreen = Portal1Visibility.isVisible || Portal2Visibility.isVisible;
        if (screensAreOnScreen == false && previousScreensAreOnScreen == true)
        {
            screensAreEnabled = !screensAreEnabled;
            //randomly enable screens with frequency 0.5
            //screensAreEnabled = Random.value < 0.5;


        }
        previousScreensAreOnScreen = screensAreOnScreen;

        if (!inFrontOfScreens)
        {
            screensAreEnabled = false;
        }


        if (screensAreEnabled)
        {
            Portal1Screen.material = Portal2Material;
            Portal2Screen.material = Portal1Material;
            Portal1Collider.enabled = true;
            Portal2Collider.enabled = true;
        }
        else
        {
            Portal1Screen.material = TransparentMaterial;
            Portal2Screen.material = TransparentMaterial;
            Portal1Collider.enabled = false;
            Portal2Collider.enabled = false;
        }

    }


}
