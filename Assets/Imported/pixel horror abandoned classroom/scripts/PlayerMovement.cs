using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{

    private CharacterController controller;
     public float speed = 12f;
    public float gravity = -9.81f;
    public float  JumpHight = 3; 
    Vector3 V;
    private void Awake()
    {
        controller = gameObject.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {

 
        if(controller.isGrounded&& V.y<0) 
        {
            V.y = -2f;
        }
        float X = Input.GetAxis("Horizontal");
        float Z = Input.GetAxis("Vertical");
        Vector3 M = transform.right * X + transform.forward*Z;
        controller.Move(M*speed*Time.deltaTime);
        if(Input.GetButtonDown("Jump"))
        {
            V.y = Mathf.Sqrt(JumpHight * -2f * gravity);
        }
        V.y += gravity * Time.deltaTime;
        controller.Move(V * Time.deltaTime);


    }

}
