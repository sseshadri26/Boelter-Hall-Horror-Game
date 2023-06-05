using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximityActivatedFlickeringLight : MonoBehaviour
{

    [Tooltip("The chance that this light is flickering")]
    [SerializeField]
    [Range(0f, 1f)]
    private float flickeringChance = 1f;

    [Tooltip("How often this light is on when flickering")]
    [SerializeField]
    [Range(0f, 1f)]
    private float onChance = 0.5f;

    [Tooltip("The chance we actually toggle the light we are about to toggle it")]
    [SerializeField]
    [Range(0f, 1f)]
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
    [SerializeField]
    private Light otherLight;

    private MeshRenderer mr;

    private BoxCollider bc;

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


        ourLight.enabled = false;
        otherLight.enabled = false;
        if (affectShader)
        {
            mr.material.shader = offShader;
        }

        bc = GetComponent<BoxCollider>();


    }

    // Using FixedUpdate so that this doesn't rely on frame rate
    void FixedUpdate()
    {
        // // If the light is currently on, but both rngs tell us to toggle, then turn the light off
        // if (ourLight.enabled && Random.Range(0f, 1f) > onChance && Random.Range(0f, 1f) <= changeChance)
        // {
        //     ourLight.enabled = false;
        //     if (affectShader)
        //     {
        //         mr.material.shader = offShader;
        //     }
        // }
        // // If the light is currently off, but both rngs tell us to toggle, then turn the light on
        // else if (!ourLight.enabled && Random.Range(0f, 1f) <= onChance && Random.Range(0f, 1f) <= changeChance)
        // {
        //     ourLight.enabled = true;
        //     if (affectShader)
        //     {
        //         mr.material.shader = onShader;
        //     }
        // }
    }

    // if player hits boxx collider, rapidly flash the light on and off, then turn it on forever

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            StartCoroutine("FlashLight");
        }
    }

    // create a coroutine that flashes the light on and off

    IEnumerator FlashLight()
    {
        // turn the light on and off 10 times
        for (int i = 0; i < 10; i++)
        {
            ourLight.enabled = true;
            if (affectShader)
            {
                mr.material.shader = onShader;
            }
            // otherLight.enabled = true;

            yield return new WaitForSeconds(0.05f);
            ourLight.enabled = false;
            if (affectShader)
            {
                mr.material.shader = offShader;
            }
            // otherLight.enabled = false;

            yield return new WaitForSeconds(0.05f);
        }
        // turn the light on forever
        ourLight.enabled = true;
        otherLight.enabled = true;

        if (affectShader)
        {
            mr.material.shader = onShader;
        }
        // disable the box collider so the light doesn't flash again
        bc.enabled = false;

    }
}

