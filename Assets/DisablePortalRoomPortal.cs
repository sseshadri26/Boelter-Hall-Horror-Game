using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisablePortalRoomPortal : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject ObjectsToEnable;
    void Start()
    {


        foreach (var kvp in Globals.flags)
        {
            Debug.Log("Key: " + kvp.Key + ", Value: " + kvp.Value);
        }


        // Debug.Log(Globals.curSpawnPoint);
        // Debug.Log(Globals.inventory);
        // Debug.Log(Globals.portalPosition5F);

        if (Globals.flags["Floor5PortalRoomPortal"] != null && Globals.flags["Floor5PortalRoomPortal"] == true)
        {
            // ebable that object
            ObjectsToEnable.SetActive(true);
        }
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
