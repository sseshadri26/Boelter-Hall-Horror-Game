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

    const int c_numTabs = 3;
    const PanelAnimator.PanelAnimationSpeed c_swapSpeed = PanelAnimator.PanelAnimationSpeed.FAST;

    // UI Tags
    const string k_currentPanelContainer = "current-panel-container";
    const string k_tabsContainer = "tabs";
    const string k_leftTabButton = "left-tab-button";
    const string k_rightTabButton = "right-tab-button";
    

    // UI References
    VisualElement m_currentPanelContainer;
    SelectableScrollView m_tabsContainer;
    Button m_leftTabButton;
    Button m_rightTabButton;

    void Awake()
    {
        m_currentPanelContainer = document.rootVisualElement.Q<VisualElement>(k_currentPanelContainer);
        m_tabsContainer = document.rootVisualElement.Q<SelectableScrollView>(k_tabsContainer);
        m_leftTabButton = document.rootVisualElement.Q<Button>(k_leftTabButton);
        m_rightTabButton = document.rootVisualElement.Q<Button>(k_rightTabButton);

        m_leftTabButton.RegisterCallback<ClickEvent>(ev => TabLeft());
        m_rightTabButton.RegisterCallback<ClickEvent>(ev => TabRight());

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

            // Don't force width since it messes with dimensions -- unsure why; should figure out
        }

        for(int i = 0; i < tabVisuals.Count; i++)
        {
            int i_cached = i;
            tabVisuals[i].RegisterCallback<ClickEvent>(ev => 
            {
                SwapToTab(i_cached);
            });
        }

        // Automatically select the first tab
        m_tabsContainer.VisuallySelectOne(tabVisuals[0]);
        tabPanelAnimators[0].InstantOpen();

    }

    private void SwapToTab(int tabIndex)
    {
        tabPanelAnimators[currentTab].AnimateClose(PanelAnimator.PanelPosition.CENTER, c_swapSpeed);
        tabPanelAnimators[tabIndex].AnimateOpen(PanelAnimator.PanelPosition.CENTER, c_swapSpeed);
        m_tabsContainer.VisuallySelectOne(tabVisuals[tabIndex]);
        currentTab = tabIndex;
    }
    public void TabLeft()
    {
        if(currentTab <= 0) return;
        SwapToTab(currentTab - 1);
    }

    public void TabRight()
    {
        if(currentTab >= c_numTabs - 1) return;
        SwapToTab(currentTab + 1);
    }
}
