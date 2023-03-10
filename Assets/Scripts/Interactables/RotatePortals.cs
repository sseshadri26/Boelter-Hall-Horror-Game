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

    [SerializeField] private int[] numberOfPortalPairs;

    [SerializeField] private GameObject[] portalGroup1;
    [SerializeField] private GameObject[] portalGroup2;



    //create array of size numberOfPortalConfigs, each of which can hold any number of portals (gameobjects)

    // If door is activated, load next scene.
    public void Start()
    {

    }
    public void Activate()
    {
        // Prevent turning twice in a row
        if (activated)
        {
            return;
        }

        activated = true;
        transform.DOBlendableRotateBy(Vector3.zero, 1).OnComplete(() => activated = false);

        curPortalPair++;
        Globals.portalPosition5F++;
        if (curPortalPair == numberOfPortalConfigs) curPortalPair = Globals.portalPosition5F = 0;

        // Disable each portal pair then enable only the portal pair we want
        //foreach (Transform child in transform)
        //{
        //    child.gameObject.SetActive(false);
        //}
        //transform.GetChild(curPortalPair).gameObject.SetActive(true);

        switch (curPortalPair)
        {
            case 0:
                foreach (GameObject portal in portalGroup1)
                {
                    portal.SetActive(true);
                }
                foreach (GameObject portal in portalGroup2)
                {
                    portal.SetActive(false);
                }
                break;
            case 1:
                foreach (GameObject portal in portalGroup1)
                {
                    portal.SetActive(false);
                }
                foreach (GameObject portal in portalGroup2)
                {
                    portal.SetActive(true);
                }
                break;
        }
    }
}



