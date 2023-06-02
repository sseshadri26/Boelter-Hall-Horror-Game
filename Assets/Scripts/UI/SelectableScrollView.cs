using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;

[System.Obsolete("Use ItemSelector instead")]
public class SelectableScrollView : ScrollView
{
    public new class UxmlFactory : UxmlFactory<SelectableScrollView, UxmlTraits> { }

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

    public SelectableScrollView()
    {
        // Delay execution to allow children in UI to be populated after UI update frame (is this true?)
        // REMEMBER: this is not guaranteed to occur before Awake or Start!
        schedule.Execute(() =>
        {
            foreach (VisualElement child in contentContainer.Children())
            {
                child.RegisterCallback<ClickEvent>(ev =>
                {
                    if (automaticallyVisuallySelect)
                        VisuallySelectOne(child);
                });
            }
        });
    }

    public new void Add(VisualElement child)
    {
        base.Add(child);    // Update the true version

        child.RegisterCallback<ClickEvent>(ev =>
        {
            if (automaticallyVisuallySelect)
                VisuallySelectOne(child);
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
    private List<VisualElement> GetItems() => this.Children().ToList();
}
