using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;


/// <summary>
/// Component for visually selecting items in a collection, provided that the items have 
/// the necessary USS implementations.
/// </summary>
public class ItemSelector<SelectEventType> where SelectEventType : EventBase<SelectEventType>, new()
{
    // USS Classes

    // DESIGN CHOICE: Using USS classes to change the appearance of UI instead
    // of hard-coding it in here is a great way to maintain separation of
    // visuals and functionality -- the code isn't tightly coupled to the
    // way the UI looks, which opens up the possibility of switching out the visuals
    // to something new. This pattern was taken from Unity's open source project "Dragon Crashers"

    // Note that these are special "abstract" USS classes that can be added to the parent, and the derived
    // UI's USS handles the implementation of how it looks when selected
    const string c_Selected = "selected";
    const string c_NotSelected = "not-selected";

    public event System.Action<VisualElement> OnItemSelected;

    VisualElement itemContainer;
    public ItemSelector(VisualElement itemContainer)
    {
        this.itemContainer = itemContainer;

        // Delay execution to allow children in UI to be populated after UI update frame (is this true?)
        // REMEMBER: this is not guaranteed to occur before Awake or Start!
        this.itemContainer.schedule.Execute(() =>
        {
            foreach (VisualElement child in this.itemContainer.Children())
            {
                child.RegisterCallback<SelectEventType>(ev =>
                {
                    if (automaticallyVisuallySelect)
                        VisuallySelectOne(child);
                });
            }
        });
    }


    /// <summary>
    /// Add new item to the container while also registering it to the selection systtem
    /// </summary>
    public void AddItem(VisualElement item)
    {
        itemContainer.Add(item);

        item.RegisterCallback<SelectEventType>(ev =>
        {
            if (automaticallyVisuallySelect)
                VisuallySelectOne(item);
        });
    }

    public bool automaticallyVisuallySelect { get; set; } = true;

    int currentIndex = -1;

    /// <summary>
    /// Play the animation tied to selecting an item, and at the same time play the animation
    /// to deselect all other items (if any are selected).
    /// </summary>
    public void VisuallySelectOne(VisualElement item)
    {
        // For some reason we need
        GetItems().ForEach(VisuallyUnselect);
        VisuallySelect(item);
        currentIndex = GetItems().IndexOf(item);
    }

    /// <summary>
    /// Attempt to select the next element in the list
    /// </summary>
    public void VisuallySelectNext()
    {
        if (currentIndex < GetItems().Count - 1)
        {
            currentIndex++;
            VisuallySelectOne(GetItems()[currentIndex]);
        }
    }

    /// <summary>
    /// Attempt to select the previous element in the list
    /// </summary>
    public void VisuallySelectPrev()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            VisuallySelectOne(GetItems()[currentIndex]);
        }
    }

    /// <summary>
    /// Reset the selection 
    /// </summary>
    public void Reset()
    {
        GetItems().ForEach(VisuallyUnselect);
        currentIndex = -1;
    }

    /// <summary>
    /// Get a reference to the currently selected element, null if no element selected
    /// </summary>
    public VisualElement GetSelectedElement()
    {
        if (currentIndex >= 0 && currentIndex < GetItems().Count)
            return GetItems()[currentIndex];
        return null;
    }


    private void VisuallySelect(VisualElement item)
    {
        if (item != null)
        {
            item.RemoveFromClassList(c_NotSelected);
            item.AddToClassList(c_Selected);
            OnItemSelected?.Invoke(item);
        }
    }

    private void VisuallyUnselect(VisualElement item)
    {
        if (item != null)
        {
            item.RemoveFromClassList(c_Selected);
            item.AddToClassList(c_NotSelected);
        }

    }

    // DESIGN CHOICE: It is repeatedly creating a list from an IEnumerable, but it's
    // probably not called enough to matter. It's very convenient to have a list.
    private List<VisualElement> GetItems() => itemContainer.Children().ToList();

}
