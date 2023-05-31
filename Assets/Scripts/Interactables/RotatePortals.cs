using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

//For loading next scene on interact.
public class RotatePortals : MonoBehaviour, IAction
{
    private bool activated = false;
    private int curPortalPair = Globals.portalPosition5F;

    //number of configurations as an enum
    [SerializeField] private int numberOfPortalConfigs = 2;

    // [SerializeField] private int[] numberOfPortalPairs;

    [SerializeField] private GameObject[] portalGroup1;
    [SerializeField] private GameObject[] portalGroup2;

    [Tooltip("Which saved portal set this represents")]
    [SerializeField] private int portalSetNum = 0;

    // Load saved portal configuration.
    public void Start()
    {
        if (SaveSystem.Data != null)
        {
            curPortalPair = Globals.portalPosition5F = SaveSystem.Data.portalPosition;
            // Debug.Log("Setting portal orientation to " + Globals.portalPosition5F + " from save");
            ChangePortals();
        }
        else
        {
            curPortalPair = Globals.portalPosition5F = 0;
        }
    }

    public void Activate()
    {
        // Prevent turning twice in a row
        if (activated)
        {
            return;
        }

        activated = true;
        // Jank way of waiting 1 sec before setting activated back to false (no rotation)
        transform.DOBlendableRotateBy(Vector3.zero, 1f).OnComplete(() => activated = false);

        curPortalPair++;
        Globals.portalPosition5F++;
        if (curPortalPair == numberOfPortalConfigs) curPortalPair = Globals.portalPosition5F = 0;

        ChangePortals();
    }

    private void ChangePortals()
    {
        // Disable each portal pair then enable only the portal pair we want
        //foreach (Transform child in transform)
        //{
        //    child.gameObject.SetActive(false);
        //}
        //transform.GetChild(curPortalPair).gameObject.SetActive(true);

        // print that we are changing?
        // Debug.Log("Changing portals to " + curPortalPair);

        switch (curPortalPair)
        {
            case 0:
                foreach (GameObject portal in portalGroup1)
                {
                    // print name of portal in portal group
                    // Debug.Log(portal.name);
                    RotatingPortalDisableManager r = portal.GetComponent<RotatingPortalDisableManager>();
                    if (r != null)
                    {
                        // Debug.Log("not null");
                        r.EnableOrDisable(true);
                    }
                }
                foreach (GameObject portal in portalGroup2)
                {
                    RotatingPortalDisableManager r = portal.GetComponent<RotatingPortalDisableManager>();
                    if (r != null)
                    {
                        // Debug.Log("not null");
                        r.EnableOrDisable(false);
                    }
                }
                break;
            case 1:
                foreach (GameObject portal in portalGroup1)
                {
                    RotatingPortalDisableManager r = portal.GetComponent<RotatingPortalDisableManager>();
                    if (r != null)
                    {
                        // Debug.Log("not null");
                        r.EnableOrDisable(false);
                    }
                }
                foreach (GameObject portal in portalGroup2)
                {
                    RotatingPortalDisableManager r = portal.GetComponent<RotatingPortalDisableManager>();
                    if (r != null)
                    {
                        // Debug.Log("not null");
                        r.EnableOrDisable(true);
                    }
                }
                break;
        }
    }
}



