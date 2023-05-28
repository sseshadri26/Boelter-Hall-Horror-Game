using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Test script useful for automatically invoking a void event on start.
/// </summary>
public class VoidEventChannelInvoker : MonoBehaviour
{
    [SerializeField] VoidEventChannelSO channel;

    void Start()
    {
        channel.RaiseEvent();
    }
}
