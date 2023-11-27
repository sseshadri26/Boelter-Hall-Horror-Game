using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealWithProblemWall : MonoBehaviour
{
    // Reference to the MeshRenderer component
    private MeshRenderer meshRenderer;

    void Start()
    {
        // Get the MeshRenderer component attached to the same GameObject
        meshRenderer = GetComponent<MeshRenderer>();

        // Ensure the MeshRenderer component is not null
        if (meshRenderer == null)
        {
            Debug.LogError("MeshRenderer component not found on the object!");
        }
    }

    // This method is called when another collider enters the trigger collider
    private void OnTriggerEnter(Collider other)
    {
        // Check if the entering collider is the player
        if (other.CompareTag("Player"))
        {
            // Disable the MeshRenderer
            meshRenderer.enabled = false;
        }
    }

    // This method is called when another collider exits the trigger collider
    private void OnTriggerExit(Collider other)
    {
        // Check if the exiting collider is the player
        if (other.CompareTag("Player"))
        {
            // Enable the MeshRenderer
            meshRenderer.enabled = true;
        }
    }
}
