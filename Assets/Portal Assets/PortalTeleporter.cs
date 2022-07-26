
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTeleporter : MonoBehaviour
{

    public Transform player;
    public Transform reciever;
    public Transform transmitter;

    [SerializeField]
    private bool playerIsOverlapping = false;


    // set player Transform on Start if it is null in the editor
    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player").transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerIsOverlapping)
        {
            Vector3 portalToPlayer = player.position - transform.position;
            float dotProduct = Vector3.Dot(transform.up, portalToPlayer);

            // If this is true: The player has moved across the portal
            if (dotProduct < -0)
            {
                // Teleport them!

                Vector3 playerOffsetFromPortal = Quaternion.Inverse(transmitter.rotation) * (player.position - transmitter.position);
                player.position = reciever.position + (reciever.rotation * playerOffsetFromPortal);

                Quaternion playerRotationOffsetFromPortal = Quaternion.Inverse(transmitter.rotation) * player.rotation;
                player.rotation = reciever.rotation * playerRotationOffsetFromPortal;



                playerIsOverlapping = false;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playerIsOverlapping = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playerIsOverlapping = false;
        }
    }
}
