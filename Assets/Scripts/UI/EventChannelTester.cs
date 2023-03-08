using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventChannelTester : MonoBehaviour
{
    [SerializeField] BoolEventChannelSO eventChannel;

    void Start()
    {
        StartCoroutine(DelayedAction(1, true));
        StartCoroutine(DelayedAction(4, false));
    }

    IEnumerator DelayedAction(float delay, bool b)
    {
        yield return new WaitForSecondsRealtime(delay);
        eventChannel.RaiseEvent(b);
    }
}
