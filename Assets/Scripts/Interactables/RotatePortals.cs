using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

//For loading next scene on interact.
public class RotatePortals : MonoBehaviour, IAction
{
    private bool activated = false;
    private int curPortalPair = 0;

    // If door is activated, load next scene.
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
        if (curPortalPair == 4) curPortalPair = 0;

        // Disable each portal pair then enable only the portal pair we want
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
        transform.GetChild(curPortalPair).gameObject.SetActive(true);
    }
}
