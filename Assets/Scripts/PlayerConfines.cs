using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FirstPersonController), typeof(CapsuleCollider), typeof(Rigidbody))]
public class PlayerConfines : MonoBehaviour
{
    private FirstPersonController fpc;
    private CapsuleCollider col;
    private Rigidbody rb;
    private float heightToStand;
    private RaycastHit hit;

    public LayerMask obstacleLayers;
    public bool allowSprinting = true;
    public bool startCrouched = false;
    public float startingPitch = 0f;

    void Awake()
    {
        fpc = GetComponent<FirstPersonController>();
        col = GetComponent<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();

        heightToStand = fpc.crouchHeight * col.height;

        // When first loading the scene, move the player to the current start position
        Transform spawnPoint = GameObject.FindGameObjectWithTag("spawnPointsParent").transform.GetChild(Globals.curSpawnPoint);
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;
    }

    void Start()
    {
        // If we start crouched, crouch on start
        if(startCrouched)
        {
            fpc.Crouch(true);
        }
        fpc.pitch = startingPitch;
    }

    void Update()
    {
        if (fpc.isCrouched)
        {
            // Debug.DrawRay(transform.position, transform.up*(heightToStand+col.radius), Color.magenta);
            // If the player would collide with something if they moved up to their max height, then we can't stand.
            fpc.enableCrouch = !rb.SweepTest(transform.up, out hit, heightToStand);
            if (allowSprinting)
            {
                fpc.enableSprint = fpc.enableCrouch;
            }
        }
    }
}
