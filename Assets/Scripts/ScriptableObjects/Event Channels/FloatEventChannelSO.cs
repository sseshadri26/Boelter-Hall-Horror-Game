using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// This class is used for Events that have one int argument.
/// Example: An Achievement unlock event, where the int is the Achievement ID.
/// </summary>
[CreateAssetMenu(menuName = "ScriptableObjects/Events/Float Event Channel")]
public class FloatEventChannelSO : EventChannelSO<float>
{}
