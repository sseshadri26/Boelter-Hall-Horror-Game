using System.Collections;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Yarn.Unity;


using UnityEngine.UI;


public class BlackoutScreen : MonoBehaviour
{
    public float flickerDuration = 0.2f; // Duration of each flicker
    public int flickerCount = 5; // Number of times to flicker

    public bool flickerStart;

    private bool flickering = false;
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main; // Make sure your camera is tagged as "MainCamera" in Unity

    }

    void Update()
    {
        if (flickerStart)
        {
            StartFlicker();
            flickerStart = false;
        }
    }

    public void StartFlicker()
    {
        if (!flickering)
        {
            flickering = true;
            StartCoroutine(FlickerScreen());
        }
    }


    private IEnumerator FlickerScreen()
    {
        mainCamera.gameObject.SetActive(false);
        yield return new WaitForSeconds(1);

        for (int i = 0; i < flickerCount; i++)
        {
            mainCamera.gameObject.SetActive(false);
            yield return new WaitForSeconds(flickerDuration);
            mainCamera.gameObject.SetActive(true);
            yield return new WaitForSeconds(flickerDuration);
        }

        mainCamera.gameObject.SetActive(true);
        flickering = false;
    }
}