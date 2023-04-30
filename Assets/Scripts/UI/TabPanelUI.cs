using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;

public class TabPanelUI : MonoBehaviour
{
    [SerializeField] UIDocument document;

    [SerializeField] List<UIDocument> tabPanels = new List<UIDocument>();

    List<PanelAnimator> tabPanelAnimators;
    List<VisualElement> tabVisuals;
    int currentTab = 0;

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

        tabPanelAnimators = tabPanels.Select<UIDocument, PanelAnimator>(panel => panel.GetComponent<PanelAnimator>()).ToList();
        tabVisuals = (m_tabsContainer.contentContainer.Children()).ToList();
    }

    void Start()
    {
        foreach(UIDocument panel in tabPanels)
        {
            m_currentPanelContainer.Add(panel.rootVisualElement);

            // Ensure child panels don't overlap
            panel.rootVisualElement.style.position = Position.Absolute;

            // Ensure child panels don't scale past size of container
            panel.rootVisualElement.style.height = Length.Percent(100);
            panel.rootVisualElement.style.width = Length.Percent(100);
        }

        // Automatically select the first tab
        m_tabsContainer.VisuallySelectOne(tabVisuals[0]);

        for(int i = 0; i < tabVisuals.Count; i++)
        {
            int i_cached = i;
            tabVisuals[i].RegisterCallback<ClickEvent>(ev => 
            {
                tabPanelAnimators[i_cached].InstantOpen();
                tabPanelAnimators[currentTab].InstantClose();
                //tabPanelAnimators[i_cached].AnimateOpen(PanelAnimator.PanelPosition.LEFT, PanelAnimator.PanelAnimationSpeed.FAST);
                //tabPanelAnimators[currentTab].AnimateClose(PanelAnimator.PanelPosition.TOP, PanelAnimator.PanelAnimationSpeed.FAST);
                currentTab = i_cached;
            });
        }

        // Ensures that positioning follows rules of the panel container (such as center alignment)
        //tabPanel.rootVisualElement.style.position = Position.Relative;
    }
}
