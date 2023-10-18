using UnityEngine;
using System.Collections;

public class BlackoutEffect : MonoBehaviour
{
    public float fadeSpeed = 2.0f; // Adjust this to control the speed of the blackout effect.
    public float blackoutDuration = 1.0f; // Adjust this to control how long the blackout lasts.

    private CanvasGroup canvasGroup;

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void TriggerBlackout()
    {
        StartCoroutine(DoBlackout());
    }

    private IEnumerator DoBlackout()
    {
        float elapsedTime = 0;

        while (elapsedTime < blackoutDuration)
        {
            // Calculate the alpha value to create the flickering effect.
            float alpha = Mathf.PingPong(elapsedTime * fadeSpeed, 1f);

            canvasGroup.alpha = alpha;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the image is completely black when the effect is done.
        canvasGroup.alpha = 1f;
    }
}
