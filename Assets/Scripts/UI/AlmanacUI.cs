using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class AlmanacUI : PanelUI
{
    [Header("Inventory Properties")]
    [SerializeField] VisualTreeAsset inventoryItemUI = default;
    [SerializeField] InventorySO inventory = default;
    [SerializeField] float scrollSpeed = 300;
    [SerializeField] float scrollButtonJumpSize = 100;
    
    [Header("Item Name Colors")]
    // DESIGN CHOICE: Make a separate field for each, instead of using some kind of
    // list containing data structures that pair an item status with a color since
    // it is unlikely that very many more item statuses will be introduced, thus making
    // it not really worth the cost of the added complexity
    [SerializeField] Color normalItemColor = Color.white;
    [SerializeField] Color keyItemColor = Color.blue;

    // UI Tags
    const string k_ItemList = "item-list";
    const string k_ItemTitle = "item-title";
    const string k_ItemVisual = "item-visual";
    const string k_ItemDesc = "item-desc";
    const string k_ScrollUpButton = "scroll-up-button";
    const string k_ScrollDownButton = "scroll-down-button";

    // UI Tags (Inventory Item)
    const string k_ItemRoot = "item-root";
    const string k_ItemName = "item-name";
    const string k_ItemGraphic = "item-graphic";


    // AlmanacUI USS Classes

    // DESIGN CHOIE: Using USS classes to change the appearance of UI instead
    // of hard-coding it in here is a great way to maintain separation of
    // visuals and functionality -- the code isn't tightly coupled to the
    // way the UI looks, which opens up the possibility of switching out the visuals
    // to something new. This pattern was taken from Unity's open source project "Dragon Crashers"

    const string c_Selected = "selected";
    const string c_NotSelected = "not-selected";
    const string c_Marked = "marked";
    const string c_NotMarked = "not-marked";

    // UI References
    ScrollView m_ItemList = default;

    Label m_ItemTitle = default;
    VisualElement m_ItemVisual = default;
    Label m_ItemDesc = default;

    Button m_InventoryScrollUpButton = default;
    Button m_InventoryScrollDownButton = default;

    protected override void Awake()
    {
        base.Awake();

        m_ItemList = root.Q<ScrollView>(k_ItemList);

        m_ItemTitle = root.Q<Label>(k_ItemTitle);
        m_ItemVisual = root.Q<VisualElement>(k_ItemVisual);
        m_ItemDesc = root.Q<Label>(k_ItemDesc);

        m_InventoryScrollUpButton = root.Q<Button>(k_ScrollUpButton);
        m_InventoryScrollDownButton = root.Q<Button>(k_ScrollDownButton);


        // Register Callbacks
        m_InventoryScrollUpButton.RegisterCallback<ClickEvent>(ev => m_ItemList.verticalScroller.ScrollPageUp());
        m_InventoryScrollDownButton.RegisterCallback<ClickEvent>(ev => m_ItemList.verticalScroller.ScrollPageDown()); 

        m_ItemList.RegisterCallback<WheelEvent>(SpeedUpScroll);
        m_ItemList.verticalPageSize = scrollButtonJumpSize;
        UpdateDisplay(inventory.items);
    }


    // This is a function to accelerate the scroll speed, and is a work-around for Unity's currently slightly
    // buggy scrolling system. Here's the post that inspired it: https://forum.unity.com/threads/listview-mousewheel-scrolling-speed.1167404/ -- Specifically in leanon00's reply
    private void SpeedUpScroll(WheelEvent ev)
    {
        m_ItemList.scrollOffset = new Vector2(0, m_ItemList.scrollOffset.y + scrollSpeed * ev.delta.y);
    }

    protected override void OnOpenPanel()
    {
        // Instead of registering callback to inventory, simply activate when panel opens
        UpdateDisplay(inventory.items);
    }

    private void UpdateDisplay(List<InventoryItemSO> itemList)
    {
        // DESIGN CHOICE: Update display by clearing every single item then reinstantiating
        // new list instead of pooling items. Why? Well it's simple, and we probably
        // won't be calling this function A LOT, making it not too bad of a cost.

        m_ItemList.Clear();
        m_ItemList.scrollOffset = Vector2.zero;

        bool isFirstItem = true;
        foreach(InventoryItemSO itemData in itemList)
        {
            TemplateContainer instance = inventoryItemUI.Instantiate();
            InitializeItemElement(instance, itemData);

            m_ItemList.Add(instance);

            // DESIGN CHOICE: Inventory item should be fully initialized and
            // already inside of the list by the time the initial "click" is
            // simulated, since the logic expects it to be in the list
            if(isFirstItem)
            {
                HandleInventoryItemClicked(instance, itemData);
                isFirstItem = false;
            }

        }
    }

    private void InitializeItemElement(VisualElement item, InventoryItemSO itemData)
    {
        item.Q<Label>(k_ItemName).text = itemData.itemName;
        item.Q<Label>(k_ItemName).style.color = GetItemStatusColor(itemData);
        item.Q<VisualElement>(k_ItemGraphic).style.backgroundImage = new StyleBackground(itemData.graphic);

        // DESIGN CHOICE: Store item reference in callback instead of using generic
        // callback and searching for item index within function to reduce chances
        // of erroneously selecting wrong item
        item.RegisterCallback<ClickEvent>(ev => HandleInventoryItemClicked(item, itemData));
    }

    private void HandleInventoryItemClicked(VisualElement item, InventoryItemSO itemData)
    {
        // Visually select item
        VisuallySelectOne(item.Q<VisualElement>(k_ItemRoot));
        m_ItemList.ScrollTo(item);
        DisplayItemInformation(itemData);
    }

    /// <summary>
    /// Play the animation tied to selecting an item, and at the same time play the animation
    /// to deselect all other items (if any are selected).
    /// </summary>
    private void VisuallySelectOne(VisualElement item)
    {
        GetInventoryItems().ForEach(VisuallyUnselect);
        VisuallySelect(item);
    }
    private void VisuallySelect(VisualElement item)
    {
        if(item != null)
        {
            item.RemoveFromClassList(c_NotSelected);
            item.AddToClassList(c_Selected);
        }
    }

    private void VisuallyUnselect(VisualElement item)
    {
        if(item != null)
        {
            item.RemoveFromClassList(c_Selected);
            item.AddToClassList(c_NotSelected);
        }
            
    }


    private UQueryBuilder<VisualElement> GetInventoryItems() => root.Query<VisualElement>(k_ItemRoot);


    ///<summary>
    /// Displays the given item in the inventory's main panel.
    ///</summary>
    private void DisplayItemInformation(InventoryItemSO item)
    {
        m_ItemTitle.text = item.itemName;
        m_ItemTitle.style.color = GetItemStatusColor(item);
        m_ItemDesc.text = item.description;
        m_ItemVisual.style.backgroundImage = new StyleBackground(item.graphic);
    }

    ///<summary>
    /// Get the color corresponding to the status of this item
    ///</summary>
    private Color GetItemStatusColor(InventoryItemSO item)
    {
        // DESIGN CHOICE: Use a method to ensure consistent assignment of colors
        // DESIGN CHOICE: Use switch statement instead of dictionary for simplicity; probably won't have any additional item statuses anyways
        switch (item.itemStatus)
        {
            case InventoryItemSO.ItemStatus.NORMAL:
                return normalItemColor;
            case InventoryItemSO.ItemStatus.KEY_ITEM:
                return keyItemColor;
            default:
                return normalItemColor;
        }
    }
}
