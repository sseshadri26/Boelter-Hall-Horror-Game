using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingPortalDisableManager : MonoBehaviour
{
    // add gameobjects for posters 1 and 2, and the portal pair
    public GameObject Posters;
    public GameObject PortalPair;

    public bool portalIsCurrentlyEnabled;
    public bool isPermanentlyDisabled;

    public GameObject linkedPortal; //set in inspector, optional

    // objects to enable when portal is disabled 

    [SerializeField]
    public GameObject[] objectsToEnable;
    [SerializeField]
    public GameObject[] objectsToDisable;


    // Start is called before the first frame update
    void Start()
    {
        Posters.SetActive(true);
        PortalPair.SetActive(portalIsCurrentlyEnabled);
    }



    // helper variables to help disable the portals when posters are removed
    int numPostersRemoved = 0;

    // function to permanently disable
    public void PermanentlyDisable()
    {
        // print
        PermanentlyDisableHelper();
        // this is only called with the posters, so we do the fancy stuff here
        // this means calling the disable portals (so it doesnt abruptly disappear), and letting that function call the rest of the disabling functions here

    }

    public void PermanentlyDisablePreventOverflow()
    {
        // print
        isPermanentlyDisabled = true;
        Posters.SetActive(false);
        PortalPair.SetActive(false);
        portalIsCurrentlyEnabled = false;

        foreach (GameObject obj in objectsToEnable)
        {
            obj.SetActive(true);
        }
        foreach (GameObject obj in objectsToDisable)
        {
            obj.SetActive(false);
        }
        // this is only called with the posters, so we do the fancy stuff here
        // this means calling the disable portals (so it doesnt abruptly disappear), and letting that function call the rest of the disabling functions here

    }

    public void PermanentlyDisableHelper()
    {
        isPermanentlyDisabled = true;
        Posters.SetActive(false);
        PortalPair.SetActive(false);
        portalIsCurrentlyEnabled = false;

        foreach (GameObject obj in objectsToEnable)
        {
            obj.SetActive(true);
        }
        foreach (GameObject obj in objectsToDisable)
        {
            obj.SetActive(false);
        }

        // disable the linked portal if it exists
        if (linkedPortal != null)
        {
            linkedPortal.GetComponent<RotatingPortalDisableManager>().PermanentlyDisablePreventOverflow();
        }

    }

    // function to enable unless permanently disabled
    public void Enable()
    {
        if (!isPermanentlyDisabled)
        {
            // Posters.SetActive(true);
            PortalPair.SetActive(true);
            Posters.SetActive(true);
            portalIsCurrentlyEnabled = true;

            foreach (GameObject obj in objectsToEnable)
            {
                obj.SetActive(false);
            }
            foreach (GameObject obj in objectsToDisable)
            {
                obj.SetActive(true);
            }
            // enable the linked portal if it exists
            if (linkedPortal != null)
            {
                linkedPortal.GetComponent<RotatingPortalDisableManager>().EnablePreventOverflow();
            }
        }
    }


    public void EnablePreventOverflow()
    {
        if (!isPermanentlyDisabled)
        {
            // Posters.SetActive(true);
            PortalPair.SetActive(true);
            Posters.SetActive(true);
            portalIsCurrentlyEnabled = true;

            foreach (GameObject obj in objectsToEnable)
            {
                obj.SetActive(false);
            }
            foreach (GameObject obj in objectsToDisable)
            {
                obj.SetActive(true);
            }
        }
    }

    // function to disable unless permanently disabled
    public void Disable()
    {
        if (!isPermanentlyDisabled)
        {
            // Posters.SetActive(false);
            PortalPair.SetActive(false);
            Posters.SetActive(false);
            portalIsCurrentlyEnabled = false;


            foreach (GameObject obj in objectsToEnable)
            {
                obj.SetActive(true);
            }
            foreach (GameObject obj in objectsToDisable)
            {
                obj.SetActive(false);
            }
            // disable the linked portal if it exists
            if (linkedPortal != null)
            {
                linkedPortal.GetComponent<RotatingPortalDisableManager>().DisablePreventOverflow();
            }


        }
    }


    public void DisablePreventOverflow()
    {
        if (!isPermanentlyDisabled)
        {
            // Posters.SetActive(false);
            PortalPair.SetActive(false);
            Posters.SetActive(false);
            portalIsCurrentlyEnabled = false;

            foreach (GameObject obj in objectsToEnable)
            {
                obj.SetActive(true);
            }
            foreach (GameObject obj in objectsToDisable)
            {
                obj.SetActive(false);
            }

        }
    }

    // function to toggle
    public void Toggle()
    {
        if (portalIsCurrentlyEnabled)
        {
            Disable();
        }
        else
        {
            Enable();
        }
    }

    // function for eneable or disable taking in a bool
    public void EnableOrDisable(bool enable)
    {
        if (enable)
        {
            Enable();
        }
        else
        {
            Disable();
        }
    }

}
