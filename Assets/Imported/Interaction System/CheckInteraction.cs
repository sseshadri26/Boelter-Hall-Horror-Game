using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FirstPersonController))]
public class CheckInteraction : MonoBehaviour
{
    /*Solution by GameDevTraum (edited for Input System by Cole Strain)
    * 
    * Article: https://gamedevtraum.com/gdt-short/basic-interaction-system-for-unity/
    * Website: https://gamedevtraum.com/en/
    * Channel: https://youtube.com/c/GameDevTraum
    * 
    * Visit the website to find more articles, solutions and assets
    */

    [SerializeField]
    private float minInteractionDistance;
   
    [SerializeField]
    private GameObject rayOrigin;


    private Ray ray;

    private bool canInteract;

    private InteractionReceiver currentReceiver;


    private void Start()
    {
        GetComponent<FirstPersonController>().controls.Interact.started += ctx => Interact();
    }

    private void Update()
    {
        CheckRaycast();
    }

    private void Interact()
    {
        if (canInteract)
        {
            // In this region the character is seeing an object with which he can interact
            currentReceiver.Activate();
        }
    }

    private void CheckRaycast()
    {
        ray = new Ray(rayOrigin.transform.position, rayOrigin.transform.forward);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
           

            if (hit.distance < minInteractionDistance)
            {


                currentReceiver = hit.transform.gameObject.GetComponent<InteractionReceiver>();

                if (currentReceiver != null)
                {
                    //Here you can make something with the interact message

                    Debug.Log(currentReceiver.GetInteractionMessage());
 
                    canInteract = true;
                   
                }
                else
                {
                    canInteract = false;
                }
            }
        }

      
    }

}
