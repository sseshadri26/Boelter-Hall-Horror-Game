using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    [SerializeField]
    private TextMeshProUGUI displayText;

    private Ray ray;

    private bool canInteract;

    private InteractionReceiver currentReceiver;

    private bool holdingButton = false;

    private float holdingTime = 0f;


    private void Start()
    {
        GetComponent<FirstPersonController>().controls.Interact.started += ctx => Interact();
        GetComponent<FirstPersonController>().controls.Interact.canceled += ctx => CancelHold();
    }

    private void OnDisable()
    {
        GetComponent<FirstPersonController>().controls.Interact.started -= ctx => Interact();
        GetComponent<FirstPersonController>().controls.Interact.canceled -= ctx => CancelHold();
    }

    private void Update()
    {
        CheckRaycast();
        holdingTime += Time.deltaTime;
    }

    private void Interact()
    {
        if (canInteract)
        {
            // In this region the character is seeing an object with which he can interact

            if (currentReceiver.holdToInteract)
            {
                holdingButton = true;
                holdingTime = 0f;
                StartCoroutine("WaitForHolding", currentReceiver.howLongToHold);
            }
            else if (currentReceiver)
            {
                currentReceiver.Activate();
            }
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

                if (currentReceiver != null && currentReceiver.enabled)
                {
                    //Here you can make something with the interact message

                    // Debug.Log(currentReceiver.GetInteractionMessage());
                    if (displayText)
                    {
                        displayText.text = currentReceiver.GetInteractionMessage();
                    }

                    canInteract = true;
                    return;
                }
            }
        }

        canInteract = false;
        displayText.text = "";
    }

    private void CancelHold()
    {
        if (currentReceiver && currentReceiver.holdToInteract)
        {
            holdingButton = false;
            holdingTime = 0f;
            StopAllCoroutines();
        }
    }

    private IEnumerator WaitForHolding(float targetTime)
    {
        while (holdingTime < targetTime && holdingButton && currentReceiver != null)
        {
            // Call the `Animate()` function of the `currentReceiver` object
            currentReceiver.Animate();

            // Wait for a short time before checking again
            yield return new WaitForSeconds(0.1f);
        }

        if (holdingButton && currentReceiver != null)
        {
            currentReceiver.Activate();
        }
    }

}
