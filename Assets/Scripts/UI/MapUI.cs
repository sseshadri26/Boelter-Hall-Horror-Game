using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MapUI : PanelUI
{
    // UI Tags
    const string k_map = "map";

    // UI References
    VisualElement m_map;

    protected override void Awake()
    {
        base.Awake();

        m_map = root.Q<VisualElement>(k_map);

        // NOTE: As of now, there's nothing we do with the map since
        // it's automatically assigned a graphic in the UXML, but perhaps
        // there's something we want to do to it later
    }
}