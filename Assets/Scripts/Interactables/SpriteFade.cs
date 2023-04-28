using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFade : MonoBehaviour
{
    // Fades out a sprite if the player collides with it

    public float fadeDuration = 0.5f; // Duration of the fade in seconds
    public Transform player; 

    private Renderer renderer;
    private float alpha = 1f;
    private float fadeTimer = 0f;
    private bool isPlayerUnderneath = false;

    void Start()
    {
        renderer = GetComponent<Renderer>();
    }

    void Update()
    {
        if (isPlayerUnderneath && fadeTimer < fadeDuration)
        {
            fadeTimer += Time.deltaTime;
            alpha = 1f - (fadeTimer / fadeDuration);
            Color color = renderer.material.color;
            color.a = alpha;
            renderer.material.color = color;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerUnderneath = true;
        }
    }
}
