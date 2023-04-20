using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TabPanelUI : MonoBehaviour
{
    [SerializeField] UIDocument document;
    [SerializeField] UIDocument tabPanel;

    // UI Tags
    const string k_currentPanelContainer = "current-panel-container";

    // UI References
    VisualElement m_currentPanelContainer;
    void Awake()
    {
        m_currentPanelContainer = document.rootVisualElement.Q<VisualElement>(k_currentPanelContainer);

    }

    void Start()
    {
        m_currentPanelContainer.Add(tabPanel.rootVisualElement);

        // Ensures that positioning follows rules of the panel container (such as center alignment)
        tabPanel.rootVisualElement.style.position = Position.Relative;
    }
}
