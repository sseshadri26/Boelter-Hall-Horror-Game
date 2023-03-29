using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableOnInteract : MonoBehaviour, IAction
{

    public bool activated;

    DisableOnInteract OtherScript;

    [SerializeField]
    public GameObject PortalPair;

    [SerializeField]
    public GameObject PosterToDelete;

    [SerializeField]
    public GameObject OtherPoster;

    //Posters spawn unactivated
    void Start()
    {
        activated = false;
    }

    /*
    On interact, activate poster, remove poster from wall, and if the
    corresponding poster in the pair has been activated, disable portal pair
    */

    public void Activate()
    {
        if (OtherPoster != null)
        {
            OtherScript = OtherPoster.GetComponent<DisableOnInteract>();
        }

        if (!activated)
        {
            activated = true;



            if (OtherScript == null || OtherScript.activated)
            {
                Debug.Log("pass");
                if (PortalPair != null)
                {
                    PortalPair.GetComponent<DisablePortals>().enabled = true;
                }
            }

            PosterToDelete.SetActive(false);
        }
    }



}
