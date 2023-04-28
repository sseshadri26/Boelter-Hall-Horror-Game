using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class FMODTest : MonoBehaviour
{
    int timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        FMODManager.Instance.PlaySound(FMODManager.SFX.door_close);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timer++;
        if (timer > 50)
        {
            FMODManager.Instance.PlaySound(FMODManager.SFX.door_close);
            timer = 0;
        }
    }
}
