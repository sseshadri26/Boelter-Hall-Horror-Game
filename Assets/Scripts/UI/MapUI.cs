using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MapUI : MonoBehaviour, IDirectionControllable
{
    [SerializeField] UIDocument document;
    // UI Tags
    const string k_map = "map";

    // UI References
    VisualElement m_map;

    void Awake()
    {
        m_map = document.rootVisualElement.Q<VisualElement>(k_map);

        // NOTE: As of now, there's nothing we do with the map since
        // it's automatically assigned a graphic in the UXML, but perhaps
        // there's something we want to do to it later
    }

    public void MoveUp()
    {
        // Don't do anything for now
    }

    public void MoveDown()
    {
        // Don't do anything for now
    }

    public void MoveLeft()
    {
        // Don't do anything for now
    }

    public void MoveRight()
    {
        // Don't do anything for now
    }

    public void Submit()
    {
        // Don't do anything for now
    }
}
