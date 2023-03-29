using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(menuName = "ScriptableObjects/Item UI Generators/Plain")]
public class PlainItemUIGeneratorSO : ItemUIGeneratorSO
{
    // UI Tags (Inventory Item)
    const string k_ItemName = "item-name";

    [SerializeField] PlainItemCollectionSO plainItemCollectionSO = default;
    [SerializeField] VisualTreeAsset plainItemUI = default;
    public override List<ItemUIResult> GenerateUI()
    {
        List<ItemUIResult> results = new List<ItemUIResult>();
        foreach(ItemSO item in plainItemCollectionSO.items)
        {
            results.Add(new ItemUIResult{reference = item, ui = GenerateListItemUI(item)});
        }
        return results;
    }

    private TemplateContainer GenerateListItemUI(ItemSO itemData)
    {
        TemplateContainer instance = plainItemUI.Instantiate();
        instance.Q<Label>(k_ItemName).text = itemData.itemName;
        return instance;
    }
}
