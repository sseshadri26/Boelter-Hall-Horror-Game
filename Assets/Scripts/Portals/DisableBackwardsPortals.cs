using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableBackwardsPortals : MonoBehaviour
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
    PortalTeleporter Portal1Teleporter;
    [SerializeReference]
    PortalTeleporter Portal2Teleporter;


    Material Portal1Material;
    Material Portal2Material;
    [SerializeReference]
    Material TransparentMaterial;

    // Start is called before the first frame update
    void Start()
    {
        Portal1Material = GetComponent<PortalTextureSetup>().cameraMatA;
        Portal2Material = GetComponent<PortalTextureSetup>().cameraMatB;
    }

    // Update is called once per frame
    void Update()
    {
        if (Portal1Visibility.playerIsInFrontOfPortal || Portal2Visibility.playerIsInFrontOfPortal)
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
