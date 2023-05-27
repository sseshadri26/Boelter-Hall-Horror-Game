using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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
        schedule.Execute(() => 
        {
            foreach(VisualElement child in contentContainer.Children())
            {
                child.RegisterCallback<ClickEvent>(ev => VisuallySelectOne(child));
            }
        });
    }

    public new void Add(VisualElement child)
    {
        base.Add(child);
        child.RegisterCallback<ClickEvent>(ev => VisuallySelectOne(child));
    }


    /// <summary>
    /// Play the animation tied to selecting an item, and at the same time play the animation
    /// to deselect all other items (if any are selected).
    /// </summary>
    public void VisuallySelectOne(VisualElement item)
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
            OnItemSelected?.Invoke(item);
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


    private List<VisualElement> GetInventoryItems() => new List<VisualElement>(this.Children());
}
