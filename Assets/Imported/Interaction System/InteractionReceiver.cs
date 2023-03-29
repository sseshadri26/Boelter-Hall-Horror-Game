using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class InteractionReceiver : MonoBehaviour
{
    /*ENGLISH
    *Solution by GameDevTraum
    * 
    * Article: https://gamedevtraum.com/gdt-short/basic-interaction-system-for-unity/
    * Website: https://gamedevtraum.com/en/
    * Channel: https://youtube.com/c/GameDevTraum
    * 
    * Visit the website to find more articles, solutions and assets
    */

    [SerializeField]
    private string interactMessage;

    [SerializeField]
    private GameObject[] objectsWithActions;

    [SerializeField]
    private bool destroyOnUse;

    public bool holdToInteract = false;

    public float howLongToHold = 3f;

    void Start()
    {
        if (gameObject.activeSelf && (objectsWithActions.Length == 0 || objectsWithActions[0] == null))
        {
            Debug.LogError(name + " (parent: " + transform.parent.name + ") has no Objects With Actions listed!");
        }
    }


    public void Activate()
    {
        
        foreach (GameObject o in objectsWithActions) {
            o.GetComponent<IAction>().Activate();
        }

        if (destroyOnUse) {
            Destroy(this);
        }

    }

    public string GetInteractionMessage()
    {
        return interactMessage;
    }
}
