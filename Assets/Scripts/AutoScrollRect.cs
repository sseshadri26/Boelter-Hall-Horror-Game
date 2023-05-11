using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using System.Collections;

public class AutoScrollRect : MonoBehaviour
{
    public ScrollRect scrollRect;

    public void Populate()
    {
        // Find all selectable items in the ScrollRect
        Selectable[] selectables = scrollRect.content.GetComponentsInChildren<Selectable>(false);

        StartCoroutine("ResetScrollbar");

        // Subscribe to the OnSelect event of each selectable
        foreach (Selectable selectable in selectables)
        {
            EventTrigger eventTrigger = selectable.gameObject.AddComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.Select;
            entry.callback.AddListener((eventData) => OnSelectableSelected(selectable));
            eventTrigger.triggers.Add(entry);
        }
    }

    private void OnSelectableSelected(Selectable selectable)
    {
        // Check if the pointer is over a UI element
        bool isPointerOverUIObject = EventSystem.current.IsPointerOverGameObject();
        if (isPointerOverUIObject)
        {
            return;
        }

        // Get the position of the selected item in the ScrollRect
        RectTransform selectedTransform = selectable.GetComponent<RectTransform>();
        float selectedPosition = selectedTransform.anchoredPosition.y;

        // Get the size of the ScrollRect's content
        RectTransform contentTransform = scrollRect.content.GetComponent<RectTransform>();
        float contentSize = contentTransform.sizeDelta.y;

        // Get the size of the ScrollRect's viewport
        RectTransform viewportTransform = scrollRect.viewport.GetComponent<RectTransform>();
        float viewportSize = viewportTransform.rect.height;

        // Calculate the range of the scrollbar based on the content size and viewport size
        float scrollbarRange = contentSize - viewportSize;

        // Calculate the scrollbar value based on the position of the selected item
        float scrollbarValue = Mathf.Clamp01((selectedPosition + contentSize / 2f) / scrollbarRange);

        // Set the scrollbar value of the ScrollRect
        scrollRect.verticalScrollbar.value = scrollbarValue;

        // string log = ("Selected position: " + selectedPosition + '\n');
        // log += ("Content size: " + contentSize + '\n');
        // log += ("Viewport size: " + viewportSize + '\n');
        // log += ("Scrollbar range: " + scrollbarRange + '\n');
        // log += ("Scrollbar value: " + scrollbarValue);
        // Debug.Log(log);
    }

    private IEnumerator ResetScrollbar()
    {
        // Start scrollbar at the top after a frame
        // Debug.Log("Set scrollbar to 1: " + scrollRect.verticalScrollbar.name);
        yield return 0;
        scrollRect.verticalScrollbar.value = 1f;
    }
}