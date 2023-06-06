using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        // Play different music depending on the scene that's loaded in
        switch (SceneManager.GetActiveScene().name)
        {
            case "SEAS Cafe":
                // Simpler Times music_seas 0.3
                FMODManager.Instance.ChangeMainBGM(FMODManager.SFX.music_seas, 0.2f);
                break;
            case "Intro":
            case "5F Hallway":
                // Haunted Hallways music_opening 0.4
                // if (Globals.curSpawnPoint == 0)
                // {
                //     FMODManager.Instance.ChangeMainBGM(FMODManager.SFX.music_opening, 0.2f);
                // }
                // else
                // {
                //     goto default;
                // }
                FMODManager.Instance.ChangeMainBGM(FMODManager.SFX.music_hallway);
                break;
            case "5272":
                // Muddled Thoughts music_5272 0.3
                FMODManager.Instance.ChangeMainBGM(FMODManager.SFX.music_5272, 0.2f);
                break;
            case "Stairwell":
                // Outside these Walls FULL
                FMODManager.Instance.ChangeMainBGM(FMODManager.SFX.music_stairwell, 0.1f);
                break;
            case "Porter Room":
            case "Porter Room Alt":
                // Reaping music_porter 0.3
                FMODManager.Instance.ChangeMainBGM(FMODManager.SFX.music_porter, 0.2f);
                break;
            default:
                FMODManager.Instance.ChangeMainBGM(FMODManager.SFX.music_hallway);
                break;
        }
    }

    void FixedUpdate()
    {
        if (player == null)
        {
            return;
        }

        prevSpeed3 = prevSpeed2;
        prevSpeed2 = prevSpeed1;
        Vector3 horizontalVelocity = new Vector3(player.rb.velocity.x, 0, player.rb.velocity.z);
        prevSpeed1 = horizontalVelocity.magnitude; // Usual speed is from 4-9.
        float speed = (prevSpeed1 + prevSpeed2 + prevSpeed3) / 3f;

        footstepParams.pitch = Mathf.Lerp(0.9f, 0.95f, (speed - 4f) / (9f - 4f));
        footstepParams.volumePercent = Mathf.Lerp(0.6f, 1.0f, (speed - 4f) / (9f - 4f));

        //FMODManager.Instance.ModifyParams(footstepID, ref footstepParams);
        //
        playSteps(speed);
    }

    void playSteps(float speed)
    {
        //float interval = Mathf.Lerp(0.75f, 0.15f, (speed - 4.8f) / (14.88f));
        float interval = 0.75f / (speed / 4.8f);
        // 4.8 - 19.68
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
