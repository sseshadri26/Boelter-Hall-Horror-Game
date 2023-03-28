using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A persistent reference to player input accessible at all times from all parts of the project without
/// any runtime dependencies.
/// </summary>
[CreateAssetMenu(menuName = "ScriptableObjects/Input/First Person")]
public class FirstPersonActionsSO : ScriptableObject
{
    private Controls.FirstPersonActions _controls = default;
    public Controls.FirstPersonActions controls => _controls;
    void OnEnable()
    {
        _controls = new Controls.FirstPersonActions(new Controls());
    }

}
