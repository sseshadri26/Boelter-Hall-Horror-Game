using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventChannelSO<T> : ScriptableObject
{
    // DESIGN CHOICE: Simplify testing event channels by including a logging option.
    // It's one additional check and field to worry about but, the overhead is so small
    // for the benefits it provides.
    [SerializeField] private bool log = false;
    public UnityAction<T> OnEventRaised;

    public void RaiseEvent(T value)
    {
        if(OnEventRaised != null)
            OnEventRaised?.Invoke(value);
        
        if(log)
            Debug.LogFormat("CHANNEL: {0}\t VAL: {1}", name, value.ToString());
    }
}
