using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(menuName = "ScriptableObjects/Item UI Generators/Inventory")]
public class InventoryItemUIGeneratorSO : ItemUIGeneratorSO
{
    [Header("Inventory Properties")]
    [SerializeField] InventoryItemCollectionSO inventorySO = default;
    [SerializeField] VisualTreeAsset inventoryItemUI = default;

    // DESIGN CHOICE: Make a separate field for each, instead of using some kind of
    // list containing data structures that pair an item status with a color since
    // it is unlikely that very many more item statuses will be introduced, thus making
    // it not really worth the cost of the added complexity
    [SerializeField] Color normalItemColor = Color.white;
    [SerializeField] Color keyItemColor = Color.blue;


    // UI Tags (Inventory Item)
    const string k_InventoryItemName = "item-name";
    const string k_InventoryItemGraphic = "item-graphic";


    public override List<ItemUIResult> GenerateUI()
    {
        List<ItemUIResult> results = new List<ItemUIResult>();
        foreach(InventoryItemSO inventoryItem in inventorySO.items)
        {
            results.Add(new ItemUIResult{reference = inventoryItem, ui = GenerateListItemUI(inventoryItem)});
        }
        return results;
    }

    private TemplateContainer GenerateListItemUI(InventoryItemSO itemData)
    {
        TemplateContainer instance = inventoryItemUI.Instantiate();
        InitializeItemElement(instance, itemData);
        return instance;
    }


    private void InitializeItemElement(VisualElement item, InventoryItemSO itemData)
    {
        item.Q<Label>(k_InventoryItemName).text = itemData.itemName;
        item.Q<Label>(k_InventoryItemName).style.color = GetItemStatusColor(itemData);
        item.Q<VisualElement>(k_InventoryItemGraphic).style.backgroundImage = new StyleBackground(itemData.graphic);
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
