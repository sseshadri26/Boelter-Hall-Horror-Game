using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FixScrollRect: MonoBehaviour, IBeginDragHandler,  IDragHandler, IEndDragHandler, IScrollHandler
{
    public ScrollRect mainScroll;

    private void OnEnable()
    {
        mainScroll = GameObject.FindObjectOfType<ScrollRect>();
    }
 
    public void OnBeginDrag(PointerEventData eventData)
    {
        mainScroll.OnBeginDrag(eventData);
    }
 
    public void OnDrag(PointerEventData eventData)
    {
        mainScroll.OnDrag(eventData);
    }
 
    public void OnEndDrag(PointerEventData eventData)
    {
        mainScroll.OnEndDrag(eventData);
    }
 
    public void OnScroll(PointerEventData data)
    {
        mainScroll.OnScroll(data);
    }
}