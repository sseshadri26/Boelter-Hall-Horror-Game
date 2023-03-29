using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Generates a set of list items and their corresponding UI
/// </summary>
public abstract class ItemUIGeneratorSO : ScriptableObject
{
    // DESIGN CHOICE: 
    // Why make it a ScriptableObject? Its implementation does not care about runtime state, so making it a Monobehaviour could be misleading.
    // It will likely require Unity assets to be injected in, so a regular class would likely be unfit for this task.
    public struct ItemUIResult
    {
        public ItemSO reference;
        public TemplateContainer ui;
    }

    public abstract List<ItemUIResult> GenerateUI();
}
