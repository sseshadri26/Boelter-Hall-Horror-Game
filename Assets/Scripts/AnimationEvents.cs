using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationEvents : MonoBehaviour
{
    public TV nachTV;

    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();

        anim.SetInteger("Spawnpoint", Globals.curSpawnPoint);
    }

    public void CallNachenbergIntro()
    {
        if (nachTV == null)
        {
            Debug.LogError("Need a TV!");
        }
        FindObjectOfType<PlayerConfines>().NachenbergIntro(nachTV);
    }
}
