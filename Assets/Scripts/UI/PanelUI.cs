using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


/// <summary>
/// Base class for all panels. 
/// </summary>
public abstract class PanelUI : MonoBehaviour
{
    // DESIGN CHOICE: Why use base class instead of utility class or component?
    // It's true that base class is tightly coupled to all subclasses and that
    // every subclass will come with its functionality, but I have a feeling that
    // pretty much every subclass will need this functionality.
    // A static utility class would be difficult to use since my current implementation 
    // of animation requires keeping track of state.
    // A component could also work, but it seems that every panel class would probably
    // end up needing a reference to it. A base class is good in that the instance of the
    // class resides with every panel.

    [Header("Panel Properties")]
    [SerializeField] UIDocument document = default;
    [SerializeField] PanelPosition startPosition = PanelPosition.CENTER;

    // DESIGN CHOICE: Use a single bool channel for this event instead of
    // separate events for open and close to reduce number of scriptable
    // objects we need to handle
    [SerializeField] BoolEventChannelSO panelOpenStateChanged = default;

    // UI Position classes
    const string c_Center = "center";
    const string c_OffscreenRight = "offscreen-right";
    const string c_OffscreenLeft = "offscreen-left";
    const string c_OffscreenTop = "offscreen-top";
    const string c_OffscreenBot = "offscreen-bot";

    // Enum for setting where the panel should start
    public enum PanelPosition
    {
        CENTER, TOP, BOTTOM, LEFT, RIGHT
    }
    private Dictionary<PanelPosition, string> panelPositionClasses = new Dictionary<PanelPosition, string>()
    {
        {PanelPosition.CENTER, c_Center},
        {PanelPosition.LEFT, c_OffscreenLeft},
        {PanelPosition.RIGHT, c_OffscreenRight},
        {PanelPosition.TOP, c_OffscreenTop},
        {PanelPosition.BOTTOM, c_OffscreenBot}
    };

    private PanelPosition currentPosition = default;

    protected VisualElement root
    {
        get {
            if(document != null)
                return document.rootVisualElement;
            return null;
        }
    }

    protected virtual void Awake()
    {
        root.AddToClassList(panelPositionClasses[startPosition]);
        currentPosition = startPosition;

        if(panelOpenStateChanged != null)
            panelOpenStateChanged.OnEventRaised += HandlePanelOpenStateChanged;
    }

    void OnDestroy()
    {
        if(panelOpenStateChanged != null)
            panelOpenStateChanged.OnEventRaised -= HandlePanelOpenStateChanged;
    }

    private void HandlePanelOpenStateChanged(bool isOpen)
    {
        if(isOpen)
            OpenPanel();
        else
            ClosePanel();
    }

    public void OpenPanel()
    {
        ChangePosition(PanelPosition.CENTER);
        OnOpenPanel();
    }

    public void ClosePanel()
    {
        ChangePosition(startPosition);
        OnClosePanel();
    }




    // DESIGN CHOICE: Expose callbacks for subclasses to
    // perform functionality when panel closes and opens.
    // Very similar idea to OnCollisionEnter(), for example.
    protected virtual void OnOpenPanel() {}
    protected virtual void OnClosePanel() {}

    /// <summary>
    /// Helper function for animating the panel to a new position
    /// </summary>
    private void ChangePosition(PanelPosition position)
    {
        root.RemoveFromClassList(panelPositionClasses[currentPosition]);
        root.AddToClassList(panelPositionClasses[position]);
        currentPosition = position;
    }

}
