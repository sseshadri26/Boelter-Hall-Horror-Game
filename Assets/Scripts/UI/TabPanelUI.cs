using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;

public class TabPanelUI : MonoBehaviour, IDirectionControllable
{
    [SerializeField] UIDocument document;

    [SerializeField] List<TabPanelData> tabPanels = new List<TabPanelData>();

    List<PanelAnimator> tabPanelAnimators = new List<PanelAnimator>();
    List<VisualElement> tabVisuals;
    int currentTab = 0;

    int numTabs = 0;
    const PanelAnimator.PanelAnimationSpeed c_swapSpeed = PanelAnimator.PanelAnimationSpeed.FAST;

    // UI Tags
    const string k_currentPanelContainer = "current-panel-container";
    const string k_tabsContainer = "tabs";
    const string k_leftTabButton = "left-tab-button";
    const string k_rightTabButton = "right-tab-button";
    const string k_tabName = "TitleText";


    // UI References
    VisualElement m_currentPanelContainer;
    SelectableScrollView m_tabsContainer;
    Button m_leftTabButton;
    Button m_rightTabButton;


    [System.Serializable]
    public struct TabPanelData
    {
        public string name;
        public UIDocument panel;
        public PanelAnimator animator;
    }

    void Awake()
    {
        m_currentPanelContainer = document.rootVisualElement.Q<VisualElement>(k_currentPanelContainer);
        m_tabsContainer = document.rootVisualElement.Q<SelectableScrollView>(k_tabsContainer);
        m_leftTabButton = document.rootVisualElement.Q<Button>(k_leftTabButton);
        m_rightTabButton = document.rootVisualElement.Q<Button>(k_rightTabButton);

        m_leftTabButton.RegisterCallback<ClickEvent>(ev => TabLeft());
        m_rightTabButton.RegisterCallback<ClickEvent>(ev => TabRight());

        //tabPanelAnimators = tabPanels.Select<TabPanelData, PanelAnimator>(data => data.animator).ToList();
        tabVisuals = (m_tabsContainer.contentContainer.Children()).ToList();

        // Give us control over when elements are selected
        m_tabsContainer.automaticallyVisuallySelect = false;

        foreach (TabPanelData data in tabPanels)
        {
            AllocateNewTab(data);
        }

        for (int i = 0; i < tabVisuals.Count; i++)
        {
            int i_cached = i;
            tabVisuals[i].RegisterCallback<ClickEvent>(ev =>
            {
                SwapToTab(i_cached);
            });
        }

        // Automatically select the first tab (assumes there is one)
        SwapToTab(0);
    }

    // Returns ID of new tab
    public int AddNewTab(TabPanelData data)
    {
        int tabInd = AllocateNewTab(data);
        if (tabInd >= 0)
            tabPanels.Add(data);

        return tabInd;
    }

    // Returns ID of new tab
    private int AllocateNewTab(TabPanelData data)
    {
        // Max number of tabs allocated
        if (numTabs >= tabVisuals.Count)
            return -1;

        int tabIndex = numTabs;
        tabVisuals[tabIndex].Q<Label>(k_tabName).text = data.name;

        // Add Panel to panel container
        m_currentPanelContainer.Add(data.panel.rootVisualElement);

        // Ensure child panels don't overlap
        data.panel.rootVisualElement.style.position = Position.Absolute;

        // Ensure child panels don't scale past size of container
        data.panel.rootVisualElement.style.height = Length.Percent(100);

        tabPanelAnimators.Add(data.animator);

        numTabs++;
        return numTabs - 1;
    }

    private void SwapToTab(int tabIndex)
    {
        if (tabIndex >= numTabs || tabIndex < 0) return;

        tabPanelAnimators[currentTab].InstantClose();
        tabPanelAnimators[tabIndex].InstantOpen();
        m_tabsContainer.VisuallySelectOne(tabVisuals[tabIndex]);
        currentTab = tabIndex;
    }
    public void TabLeft()
    {
        if (currentTab <= 0) return;
        SwapToTab(currentTab - 1);
    }

    public void TabRight()
    {
        if (currentTab >= numTabs - 1) return;
        SwapToTab(currentTab + 1);
    }

    public void MoveUp()
    {
        // Assumes that a direction controllable component is attached to the same game object as the document
        // Given that this is one of very few GetComponent calls, probably won't hurt performance noticeably
        tabPanels[currentTab].panel.GetComponent<IDirectionControllable>()?.MoveUp();
    }

    public void MoveDown()
    {
        // Assumes that a direction controllable component is attached to the same game object as the document
        // Given that this is one of very few GetComponent calls, probably won't hurt performance noticeably
        tabPanels[currentTab].panel.GetComponent<IDirectionControllable>()?.MoveDown();
    }

    public void MoveLeft()
    {
        TabLeft();
    }

    public void MoveRight()
    {
        TabRight();
    }

    public void Submit()
    {
        // Don't do anything for now
    }
}
