using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

//For loading next scene on interact.
public class RotateModel : MonoBehaviour, IAction
{
    private bool activated = false;
    private Vector3 rotateBy = new Vector3(0, 90, 0);

    // If door is activated, load next scene.
    public void Activate()
    {
        // Prevent turning twice in a row
        if (activated)
        {
            return;
        }

        activated = true;
        transform.DOBlendableRotateBy(rotateBy, 1).OnComplete(() => activated = false);
    }
}
