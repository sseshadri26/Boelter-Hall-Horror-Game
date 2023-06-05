using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMODPlayerSFX : MonoBehaviour
{
    // Vars:
    private FirstPersonController player;
    private FMODManager.FMODParams footstepParams;
    private int footstepID;
    private float timeSinceLastStep;
    float prevSpeed1 = 0f;
    float prevSpeed2 = 0f;
    float prevSpeed3 = 0f;
    FMODManager.FMODParams spookyParams;
    int stepsTaken = 0;
    int stepsThreshold = 50;

    void Start()
    {
        player = FindObjectOfType<FirstPersonController>();
        footstepParams = new FMODManager.FMODParams(true);
        //footstepID = FMODManager.Instance.StartBGM(FMODManager.SFX.footstep_ground, false, footstepParams);
        //
        spookyParams = new FMODManager.FMODParams(true);
        spookyParams.volumePercent = 0.5f;
    }

    void FixedUpdate()
    {
        prevSpeed3 = prevSpeed2;
        prevSpeed2 = prevSpeed1;
        prevSpeed1 = player.rb.velocity.magnitude; // Usual speed is from 4-9.
        float speed = (prevSpeed1 + prevSpeed2 + prevSpeed3) / 3f;

        footstepParams.pitch = Mathf.Lerp(0.9f, 1.0f, (speed - 4f) / (9f - 4f));
        footstepParams.volumePercent = Mathf.Lerp(0.6f, 1.0f, (speed - 4f) / (9f - 4f));

        //FMODManager.Instance.ModifyParams(footstepID, ref footstepParams);
        //
        playSteps(speed);
    }

    void playSteps(float speed)
    {
        float interval = Mathf.Lerp(0.75f, 0.3f, (speed - 4f) / (5f));
        timeSinceLastStep += Time.fixedDeltaTime;

        // Check if it's time to play the footstep sound
        if (timeSinceLastStep >= interval && speed > 1)
        {
            FMODManager.Instance.PlaySound(FMODManager.SFX.footstep_ground2, false, footstepParams);
            timeSinceLastStep = 0f; // Reset the timer
            stepsTaken++;

            if (stepsTaken >= stepsThreshold)
            {
                int randomNumber = Random.Range(1, 4);
                if (randomNumber == 1)
                {
                    FMODManager.Instance.PlaySound(FMODManager.SFX.spooky_1, false, spookyParams);
                }
                if (randomNumber == 2)
                {
                    FMODManager.Instance.PlaySound(FMODManager.SFX.spooky_2, false, spookyParams);
                }
                if (randomNumber == 3)
                {
                    FMODManager.Instance.PlaySound(FMODManager.SFX.spooky_3, false, spookyParams);
                }
                stepsTaken = 0;
                stepsThreshold = Random.Range(100, 250);
            }
        }
    }
}
