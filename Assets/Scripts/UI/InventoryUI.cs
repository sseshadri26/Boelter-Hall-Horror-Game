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

    // UI Tags
    const string k_ItemList = "item-list";
    const string k_ItemTitle = "item-title";
    const string k_ItemVisual = "item-visual";
    const string k_ItemDesc = "item-desc";

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

    private void Awake()
    {
        if(document != null)
            m_Root = document.rootVisualElement;

        m_ItemList = m_Root.Q<ScrollView>(k_ItemList);

        m_ItemTitle = m_Root.Q<Label>(k_ItemTitle);
        m_ItemVisual = m_Root.Q<VisualElement>(k_ItemVisual);
        m_ItemDesc = m_Root.Q<Label>(k_ItemDesc);

        UpdateDisplay(inventory.items);

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
        m_ItemDesc.text = item.description;
        m_ItemVisual.style.backgroundImage = new StyleBackground(item.graphic);
    }
}
