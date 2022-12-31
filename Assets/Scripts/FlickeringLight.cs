using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class FlickeringLight : MonoBehaviour
{
    [Tooltip("The chance that this light is flickering")]
    [SerializeField][Range(0f, 1f)]
    private float flickeringChance = 1f;

    [Tooltip("How often this light is on when flickering")]
    [SerializeField][Range(0f, 1f)]
    private float onChance = 0.5f;

    [Tooltip("The chance we actually toggle the light we are about to toggle it")]
    [SerializeField][Range(0f, 1f)]
    private float changeChance = 0.05f;

    [Tooltip("Should this change the material's shader between lit and unlit?")]
    [SerializeField]
    private bool affectShader = false;

    [Tooltip("Shader used when light is OFF")]
    [SerializeField]
    private Shader offShader;

    [Tooltip("Shader used when light is ON")]
    [SerializeField]
    private Shader onShader;

    private Light ourLight;
    private MeshRenderer mr;

    // Start is called before the first frame update
    void Start()
    {
        // If this rng is not inside the flickeringChance, disable this component so the light doesn't flicker
        if (Random.Range(0f, 1f) >= flickeringChance)
        {
            this.enabled = false;
            return;
        }

        ourLight = GetComponent<Light>();
        if (affectShader)
        {
            mr = GetComponent<MeshRenderer>();
            if (!mr)
            {
                Debug.LogError("There is no MeshRenderer on " + gameObject.name + " for the FlickeringLight to use!");
                affectShader = false;
            }
        }

        // Initialize the light's state
        if (Random.Range(0f, 1f) <= onChance)
        {
            ourLight.enabled = true;
            if (affectShader) 
            {
                mr.material.shader = onShader;
            }
        }
        
    }

    // Using FixedUpdate so that this doesn't rely on frame rate
    void FixedUpdate()
    {
        // If the light is currently on, but both rngs tell us to toggle, then turn the light off
        if (ourLight.enabled && Random.Range(0f, 1f) > onChance && Random.Range(0f, 1f) <= changeChance)
        {
            ourLight.enabled = false;
            if (affectShader) 
            {
                mr.material.shader = offShader;
            }
        }
        // If the light is currently off, but both rngs tell us to toggle, then turn the light on
        else if (!ourLight.enabled && Random.Range(0f, 1f) <= onChance && Random.Range(0f, 1f) <= changeChance)
        {
            ourLight.enabled = true;
            if (affectShader) 
            {
                mr.material.shader = onShader;
            }
        }
    }
}
