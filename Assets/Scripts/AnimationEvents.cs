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

        // Prevent player from opening menu, breaking the cutscene
        if (Globals.curSpawnPoint == 0)
        {
            FirstPersonController.instance.controls.Inventory.Disable();
        }
    }

    public void CallNachenbergIntro()
    {
        if (nachTV == null)
        {
            Debug.LogError("Need a TV!");
        }
        FindObjectOfType<CareyCutscene>().NachenbergIntro(nachTV);
    }
}
