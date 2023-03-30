using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// Test script for enabling player input
public class InputEnabler : MonoBehaviour
{
    [SerializeField] private FirstPersonActionsSO playerInput;

    void Awake()
    {
        playerInput.controls.Enable();
    }
}
