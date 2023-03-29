using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class ItemUIProvider : MonoBehaviour
{
    public struct ItemUIResult
    {
        public ItemSO reference;
        public TemplateContainer ui;
    }

    private List<ItemUIResult> results = null;
    public List<ItemUIResult> Results
    {
        get
        {
            // DESIGN CHOICE: Lazy instantiate to ensure that this result is always ready
            // when it is needed
            if(results == null)
                results = GenerateResults();
            return results;
        }
    }


    protected abstract List<ItemUIResult> GenerateResults();
}
