using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TabPanelCollectionUI : PanelUI
{
    [Header("Tab Panel Collection Properties")]
    [SerializeField] UIDocument tabPanel;

    // UI Tags
    const string k_currentPanelContainer = "current-panel-container";

    // UI References
    VisualElement m_currentPanelContainer;
    protected override void Awake()
    {
        base.Awake();

        m_currentPanelContainer = root.Q<VisualElement>(k_currentPanelContainer);

    }

    void Start()
    {
        m_currentPanelContainer.Add(tabPanel.rootVisualElement);

        // Ensures that positioning follows rules of the panel container (such as center alignment)
        tabPanel.rootVisualElement.style.position = Position.Relative;
    }
}
