using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

//For loading next scene on interact.
public class RotateModel : MonoBehaviour, IAction
{
    private bool activated = false;
    private Vector3 rotateBy = new Vector3(0f, 180f, 0f);
    private Vector3 jumpBy = new Vector3(0, 0.25f, 0f);

    // If door is activated, load next scene.
    public void Activate()
    {
        // Prevent turning twice in a row
        if (activated)
        {
            return;
        }

        activated = true;

        // I frickin' love DOTween
        transform.DOBlendableRotateBy(rotateBy, 1).OnComplete(() => activated = false);
        transform.DOBlendableMoveBy(jumpBy, 0.5f).SetEase(Ease.OutSine).OnComplete(
            () => transform.DOBlendableMoveBy(-jumpBy, 0.5f).SetEase(Ease.OutBounce)
        );
    }
}
