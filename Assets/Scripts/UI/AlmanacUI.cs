using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class AlmanacUI : MonoBehaviour, IDirectionControllable
{
    [SerializeField] UIDocument document = default;
    [SerializeField] float scrollSpeed = 300;
    [SerializeField] float scrollButtonJumpSize = 100;

    // NOTE: This is a component-based alternative to custom styling of collection items
    [SerializeField] ItemUIGeneratorSO almanacItemUIGenerator = default;



    // UI Tags
    const string k_ItemList = "item-list";
    const string k_ItemTitle = "item-title";
    const string k_ItemVisual = "item-visual";
    const string k_ItemDesc = "item-desc";
    const string k_ScrollUpButton = "scroll-up-button";
    const string k_ScrollDownButton = "scroll-down-button";


    // UI References
    SelectableScrollView m_ItemList = default;

    Label m_ItemTitle = default;
    VisualElement m_ItemVisual = default;
    Label m_ItemDesc = default;

    Button m_InventoryScrollUpButton = default;
    Button m_InventoryScrollDownButton = default;


    // Other References
    SelectableScrollView selectableItemList = default;
    Dictionary<VisualElement, ItemSO> visualToData = new Dictionary<VisualElement, ItemSO>();

    VisualElement root
    {
        get
        {
            if (document != null)
                return document.rootVisualElement;
            return null;
        }
    }

    public void UpdateDisplay()
    {
        UpdateDisplayWithGenerator(almanacItemUIGenerator);
    }

    void Awake()
    {
        m_ItemList = root.Q<SelectableScrollView>(k_ItemList);

        m_ItemTitle = root.Q<Label>(k_ItemTitle);
        m_ItemVisual = root.Q<VisualElement>(k_ItemVisual);
        m_ItemDesc = root.Q<Label>(k_ItemDesc);

        m_InventoryScrollUpButton = root.Q<Button>(k_ScrollUpButton);
        m_InventoryScrollDownButton = root.Q<Button>(k_ScrollDownButton);


        // Register Callbacks
        m_InventoryScrollUpButton.RegisterCallback<ClickEvent>(ev => SelectPrev());
        m_InventoryScrollDownButton.RegisterCallback<ClickEvent>(ev => SelectNext());

        m_ItemList.RegisterCallback<WheelEvent>(SpeedUpScroll);
        m_ItemList.verticalPageSize = scrollButtonJumpSize;


        UpdateDisplayWithGenerator(almanacItemUIGenerator);
    }

    // This is a function to accelerate the scroll speed, and is a work-around for Unity's currently slightly
    // buggy scrolling system. Here's the post that inspired it: https://forum.unity.com/threads/listview-mousewheel-scrolling-speed.1167404/ -- Specifically in leanon00's reply
    private void SpeedUpScroll(WheelEvent ev)
    {
        m_ItemList.scrollOffset = new Vector2(0, m_ItemList.scrollOffset.y + scrollSpeed * ev.delta.y);
    }


    private void UpdateDisplayWithGenerator(ItemUIGeneratorSO generator)
    {
        // DESIGN CHOICE: Update display by clearing every single item then reinstantiating
        // new list instead of pooling items. Why? Well it's simple, and we probably
        // won't be calling this function A LOT, making it not too bad of a cost.

        m_ItemList.Clear();
        m_ItemList.scrollOffset = Vector2.zero;

        bool isFirstItem = true;

        // Generate the item UIs
        List<ItemUIGeneratorSO.ItemUIResult> results = generator.GenerateUI();

        foreach (var result in results)
        {
            //TemplateContainer instance = GenerateAlamanacListItem(itemData);
            TemplateContainer instance = result.ui;

            // Cache item data
            visualToData[instance] = result.reference;

            // DESIGN CHOICE: Store item reference in callback instead of using generic
            // callback and searching for item index within function to reduce chances
            // of erroneously selecting wrong item
            instance.RegisterCallback<ClickEvent>(ev => HandleInventoryItemClicked(instance));

            m_ItemList.Add(instance);

            // DESIGN CHOICE: Inventory item should be fully initialized and
            // already inside of the list by the time the initial "click" is
            // simulated, since the logic expects it to be in the list
            if (isFirstItem)
            {
                HandleInventoryItemClicked(instance);
                isFirstItem = false;
            }

        }
    }

    private void HandleInventoryItemClicked(VisualElement item)
    {
        // Visually select item
        m_ItemList.VisuallySelectOne(item);
        FocusOnItem(item);
    }

    private void SelectNext()
    {
        m_ItemList.VisuallySelectNext();
        VisualElement curItem = m_ItemList.GetSelectedElement();

        FocusOnItem(curItem);
    }

    private void SelectPrev()
    {
        m_ItemList.VisuallySelectPrev();
        VisualElement curItem = m_ItemList.GetSelectedElement();

        FocusOnItem(curItem);
    }

    private void FocusOnItem(VisualElement item)
    {
        if (item == null) return;
        m_ItemList.ScrollTo(item);
        DisplayItemInformation(visualToData[item]);
    }




    ///<summary>
    /// Displays the given item in the inventory's main panel.
    ///</summary>
    private void DisplayItemInformation(ItemSO item)
    {
        // DESIGN CHOICE: The reason we don't abstract this out is because the visual components of the main display
        // is pretty much the same across all derivations (title, description, image)
        m_ItemTitle.text = item.itemName;
        m_ItemDesc.text = item.description;
        m_ItemVisual.style.backgroundImage = new StyleBackground(item.graphic);
    }

    public void MoveUp()
    {
        SelectPrev();
    }

    public void MoveDown()
    {
        SelectNext();
    }

    public void MoveLeft()
    {
        // Don't do anything
    }

    public void MoveRight()
    {
        // Don't do anything
    }

    public void Submit()
    {
        // Don't do anything

        // NOTE: There's a difference between implementing a method with no behavior because
        // that's the sensible action, and implementing a method with no behavior because the
        // method itself doesn't make sense for the class. One of the downfalls of inheritance
        // is that it sometimes makes sub classes implement methods that don't make sense for
        // them. Technically using interfaces can result in the same thing, but it's a lot harder
        // for that to happen since interfaces are deliberate contracts that the class "chooses"
        // to follow so that it can be used for some specific purpose. In this case, this Alamanc
        // class deliberately "chooses" to follow this IDirectionControllable contract so that
        // it can be controlled via directional inputs. The contract makes sense for it. If it
        // didn't, then that would indicate something wrong about the system I'm using. For example,
        // if it didn't make sense for the panel to be controlled via directions, then is it really
        // even an interactable user interface?
    }

}
