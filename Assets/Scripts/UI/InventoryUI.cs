using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] UIDocument document = default;
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
    const string k_InventoryScrollUpButton = "scroll-up-button";
    const string k_InventoryScrollDownButton = "scroll-down-button";

    // UI Tags (Inventory Item)
    const string k_ItemRoot = "item-root";
    const string k_ItemName = "item-name";
    const string k_ItemGraphic = "item-graphic";
    const string k_ItemTint = "item-tint";



    // UI USS Classes
    const string c_InventoryItemClass = "inventory-item";
    const string c_InventoryItemTintClass = "inventory-item-tint";
    const string c_InventoryItemTintSelectedClass = "inventory-item-tint-selected";

    

    // UI References
    VisualElement m_Root = default;
    ScrollView m_ItemList = default;

    Label m_ItemTitle = default;
    VisualElement m_ItemVisual = default;
    Label m_ItemDesc = default;

    Button m_InventoryScrollUpButton = default;
    Button m_InventoryScrollDownButton = default;

    private void Awake()
    {
        if(document != null)
            m_Root = document.rootVisualElement;

        m_ItemList = m_Root.Q<ScrollView>(k_ItemList);

        m_ItemTitle = m_Root.Q<Label>(k_ItemTitle);
        m_ItemVisual = m_Root.Q<VisualElement>(k_ItemVisual);
        m_ItemDesc = m_Root.Q<Label>(k_ItemDesc);

        m_InventoryScrollUpButton = m_Root.Q<Button>(k_InventoryScrollUpButton);
        m_InventoryScrollDownButton = m_Root.Q<Button>(k_InventoryScrollDownButton);


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

    public void OpenInventory()
    {
        UpdateDisplay(inventory.items);
        // TODO: Fade inventory in
    }

    public void CloseInventory()
    {
        // TODO: Fade inventory out
    }

    private void UpdateDisplay(List<InventoryItemSO> itemList)
    {
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

        // Reset item highlight
        item.Q<VisualElement>(k_ItemTint).ClearClassList();
        item.Q<VisualElement>(k_ItemTint).AddToClassList(c_InventoryItemTintClass);

        // DESIGN CHOICE: Store item reference in callback instead of using generic
        // callback and searching for item index within function to reduce chances
        // of erroneously selecting wrong item
        item.RegisterCallback<ClickEvent>(ev => HandleInventoryItemClicked(item, itemData));
    }

    private void HandleInventoryItemClicked(VisualElement item, InventoryItemSO itemData)
    {
        // Visually select item
        HighlightItem(item);
        m_ItemList.ScrollTo(item);
        DisplayItemInformation(itemData);
    }


    ///<summary>
    /// Wrapper around HighlightElement to ensure consistent access of item tint element
    ///</summary>
    private void HighlightItem(VisualElement item)
    {
        if(item != null)
            HighlightElement(item.Q<VisualElement>(k_ItemTint));
    }

    private void HighlightElement(VisualElement elem)
    {
        if(elem != null)
        {
            // Unhighlight all currently selected items
            m_Root.Query<VisualElement>(k_ItemTint).
                Where(i => i.ClassListContains(c_InventoryItemTintSelectedClass)).
                ForEach(UnhighlightElement);

            // Highlight the desired item
            elem.AddToClassList(c_InventoryItemTintSelectedClass);
        }
            
    }

    private void UnhighlightElement(VisualElement elem)
    {
        if(elem != null)
            elem.RemoveFromClassList(c_InventoryItemTintSelectedClass);
    }

    private UQueryBuilder<VisualElement> GetInventoryItems() => m_Root.Query<VisualElement>(k_ItemRoot);


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
