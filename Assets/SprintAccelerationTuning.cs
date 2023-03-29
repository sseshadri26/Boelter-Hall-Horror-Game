using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprintAccelerationTuning : MonoBehaviour
{
    [SerializeField] FirstPersonController firstPersonController;

    public float startSprintSpeed = 1f;
    public float maxSprintSpeed = 10f;
    public float sprintAcceleration = 2f;

    public float unZoomedFOV = 60f;
    public float zoomedFOV = 80f;

    private bool isMovingStraight = false;
    private float currentSprintSpeed;

    public bool atFullSpeed;

    public Vector3 prevVel;

    // Start is called before the first frame update
    void Start()
    {
        currentSprintSpeed = startSprintSpeed;
        atFullSpeed = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!firstPersonController.isSprinting)
        {
            // if not sprinting, set sprint speed to the default start speed
            firstPersonController.sprintFOV = unZoomedFOV;
            currentSprintSpeed = startSprintSpeed;
            atFullSpeed = false;
            return;
        }

        Vector3 horizontalVelocity = new Vector3(firstPersonController.rb.velocity.x, 0f, firstPersonController.rb.velocity.z);

        Vector3 horizontalVelocityPrev = new Vector3(prevVel.x, 0f, prevVel.z);

        float dot = Vector3.Dot(horizontalVelocity.normalized, firstPersonController.transform.forward);

        float dotPrev = Vector3.Dot(horizontalVelocityPrev.normalized, prevVel.normalized);
        //Debug.Log("Dot: " + dot + ", prev: " + dotPrev);
        //if (((dot <= 0.1f && dot >= -0.1f) || dot >= 0.6f) && (dotPrev >= 0.999f || (dotPrev <= 0.1f && dotPrev >= -0.1f)))
        if (dot >= 0.999f || (dot >= 0.8f && dot <= 0.9f) || (dot > -0.01 && dotPrev > 0.9999f))

        {
            // print if we are going straight
            //Debug.Log(isMovingStraight);
            // player is moving in a straight line
            if (!isMovingStraight)
            {
                isMovingStraight = true;
                currentSprintSpeed = startSprintSpeed;
            }

            if (currentSprintSpeed < maxSprintSpeed)
            {
                firstPersonController.sprintFOV = unZoomedFOV;
                currentSprintSpeed += sprintAcceleration * Time.deltaTime;
                currentSprintSpeed = Mathf.Min(currentSprintSpeed, maxSprintSpeed);
            }
            if (currentSprintSpeed >= maxSprintSpeed)
            {
                atFullSpeed = true;
                firstPersonController.sprintFOV = zoomedFOV;
            }
            else
            {
                atFullSpeed = false;
            }

            firstPersonController.sprintSpeed = currentSprintSpeed;
        }
        else
        {

            Debug.Log("Dot: " + dot + ", prev: " + dotPrev);
            // player is not moving in a straight line
            isMovingStraight = false;
            currentSprintSpeed = startSprintSpeed;
        }

        prevVel = firstPersonController.rb.velocity;
    }
}
