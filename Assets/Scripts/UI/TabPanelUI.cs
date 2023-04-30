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
    const string k_tabsContainer = "tabs";
    

    // UI References
    VisualElement m_currentPanelContainer;
    SelectableScrollView m_tabsContainer;
    void Awake()
    {
        m_currentPanelContainer = document.rootVisualElement.Q<VisualElement>(k_currentPanelContainer);
        m_tabsContainer = document.rootVisualElement.Q<SelectableScrollView>(k_tabsContainer);

    }

    void Start()
    {
        m_currentPanelContainer.Add(tabPanel.rootVisualElement);

        IEnumerator<VisualElement> childPtr = m_tabsContainer.contentContainer.Children().GetEnumerator();
        childPtr.MoveNext();

        // Automatically select the first tab
        m_tabsContainer.VisuallySelectOne(childPtr.Current);

        // Ensures that positioning follows rules of the panel container (such as center alignment)
        tabPanel.rootVisualElement.style.position = Position.Relative;
    }
}
