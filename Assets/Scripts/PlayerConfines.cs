using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(FirstPersonController), typeof(CapsuleCollider), typeof(Rigidbody))]
public class PlayerConfines : MonoBehaviour
{
    private FirstPersonController fpc;
    private CapsuleCollider col;
    private Rigidbody rb;
    private float heightToStand;
    private RaycastHit hit;
    private RawImage blackScreen;

    [Tooltip("Which layers count as obstacles for uncrouching")]
    public LayerMask obstacleLayers;
    [Tooltip("Allow player to sprint or not")]
    public bool allowSprinting = true;
    [Tooltip("Start the player in a crouched position")]
    public bool startCrouched = false;
    [Tooltip("What upward angle the player will be looking at when starting. Manipulate the spawnpoint for the other angles")]
    public float startingPitch = 0f;
    [Tooltip("If the player has their vision obscured on start. Otherwise, vision fades in")]
    public bool startDark = false;
    [Tooltip("How quickly vision fades in after start. Set to 0 if you want vision to be instant")]
    public float fadeInTime = 0.75f;

    void Awake()
    {
        fpc = GetComponent<FirstPersonController>();
        col = GetComponent<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();

        heightToStand = fpc.crouchHeight * col.height;

        // Failsafe for if spawnpoint hasn't been loaded yet
        if (!Globals.loaded && SaveSystem.Data != null)
        {
            Globals.curSpawnPoint = SaveSystem.Data.playerSpawnpoint;
        }

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

        blackScreen = GameObject.FindGameObjectWithTag("BlackScreen").GetComponent<RawImage>();
        if (!startDark)
        {
            blackScreen.DOFade(0f, fadeInTime).SetUpdate(true).SetDelay(0.5f);
        }

        if (Globals.playDoorCloseSoundAtNextScene)
        {
            Globals.playDoorCloseSoundAtNextScene = false;
            FMODManager.Instance.PlaySound(FMODManager.SFX.door_close);
        }
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
