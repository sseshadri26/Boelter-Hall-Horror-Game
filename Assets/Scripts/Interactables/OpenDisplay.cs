using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

//For loading next scene on interact.
public class OpenDisplay : MonoBehaviour, IAction
{
    private Vector3 rotateBy = new Vector3(-150f, 0f, 0f);
    public BoxCollider baseHitbox;

    // If door is activated, load next scene.
    public void Activate()
    {
        // Shorten hitbox for the base so that we can access the model now
        baseHitbox.center = Vector3.zero;
        baseHitbox.size = Vector3.one;

        transform.DOBlendableRotateBy(rotateBy, 1).OnComplete(() => Destroy(this));
    }
}
