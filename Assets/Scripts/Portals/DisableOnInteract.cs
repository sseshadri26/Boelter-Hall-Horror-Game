using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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

        if (YarnFunctions.FlagIsTrue(PosterToDelete.name))
        {
            ActivateOnStart();
        }
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

            // Save flag for this poster so it's still removed when loading the scene again
            if (YarnFunctions.FlagIsTrue(PosterToDelete.name))
            {
                Debug.LogError("This poster, or one with the same name, is already flagged as taken down!\n" + PosterToDelete.name);
            }
            else
            {
                Globals.flags.Add(PosterToDelete.name, true);
            }

            if (OtherScript == null || OtherScript.activated)
            {
                Debug.Log("pass");
                if (PortalPair != null)
                {
                    PortalPair.GetComponent<DisablePortals>().enabled = true;
                    // PortalPair.GetComponent<RotatingPortalDisableManager>().PermanentlyDisable();
                }
            }

            PosterToDelete.SetActive(false);
        }
    }
    
    private void ActivateOnStart()
    {
        activated = true;
        
        if (OtherPoster != null)
        {
            OtherScript = OtherPoster.GetComponent<DisableOnInteract>();
        }
        
        if (OtherScript == null || OtherScript.activated)
        {
            Debug.Log("pass");
            if (PortalPair != null)
            {
                PortalPair.GetComponent<DisablePortals>().enabled = true;
                // PortalPair.GetComponent<RotatingPortalDisableManager>().PermanentlyDisable();
            }
        }

        PosterToDelete.SetActive(false);
    }


    // Use DOTween to animate the poster shaking. do not shake in the z axis
    public void Animate()
    {
        // Get the current rotation of the poster in global space
        Quaternion rotation = transform.localRotation;

        // Debug.Log(rotation);
        // Calculate a new shake vector that matches the orientation of the poster
        Vector3 shakeVector = new Vector3(0.15f, 0.15f);
        shakeVector = rotation * shakeVector;

        // Shake the position of the poster using the new vector
        transform.DOShakePosition(0.1f, shakeVector, 30, 90, false, true);
        // transform.DOShakePosition(0.1f, new Vector3(0, 0.15f, 0.15f), 30, 90, false, true);


    }



}
