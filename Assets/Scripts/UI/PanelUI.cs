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
    [SerializeField] PanelAnimationType animationType = PanelAnimationType.NONE;

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

    // UI Opacity classes
    // DESIGN CHOICE: Keep visibility decoupled from position to make code more understandable and flexible
    const string c_Transparent = "transparent";
    const string c_Opaque = "opaque";

    // UI Transition Classes
    const string c_AnimationFast = "animation-fast";
    const string c_AnimationInstant = "animation-instant";

    // PATHS
    const string ANIMATIONS_USS_PATH = "UI Styles/Panel-animations";

    // Enum for setting where the panel should start
    public enum PanelPosition
    {
        // Correspond with the different starting locations of the panel. If one of these options is selected,
        // the panel will slide in from a particular part of the screen.
        CENTER, TOP, BOTTOM, LEFT, RIGHT
    }
    private Dictionary<PanelPosition, string> panelPositionClasses = new Dictionary<PanelPosition, string>()
    {
        {PanelPosition.CENTER, c_Center},
        {PanelPosition.LEFT, c_OffscreenLeft},
        {PanelPosition.RIGHT, c_OffscreenRight},
        {PanelPosition.TOP, c_OffscreenTop},
        {PanelPosition.BOTTOM, c_OffscreenBot},
    };

    // TODO: Only using enums since that's how it's done for position, but this level of indirection might not be
    // necessary anymore
    public enum PanelOpacity {TRANSPARENT, OPAQUE};
    private Dictionary<PanelOpacity, string> panelVisibilityClasses = new Dictionary<PanelOpacity, string>()
    {
        {PanelOpacity.TRANSPARENT, c_Transparent},
        {PanelOpacity.OPAQUE, c_Opaque}
    };



    // Enum to make it easier for designer to select type of animation instead of worrying about panel position
    public enum PanelAnimationType
    {
        NONE, FROM_ABOVE, FROM_BELOW, FROM_LEFT, FROM_RIGHT, APPEAR, FADE_IN
    }

    private Dictionary<PanelAnimationType, PanelPosition> panelStartPosition = new Dictionary<PanelAnimationType, PanelPosition>()
    {
        {PanelAnimationType.NONE, PanelPosition.CENTER},
        {PanelAnimationType.FROM_ABOVE, PanelPosition.TOP},
        {PanelAnimationType.FROM_BELOW, PanelPosition.BOTTOM},
        {PanelAnimationType.FROM_LEFT, PanelPosition.LEFT},
        {PanelAnimationType.FROM_RIGHT, PanelPosition.RIGHT},
        {PanelAnimationType.APPEAR, PanelPosition.CENTER},
        {PanelAnimationType.FADE_IN, PanelPosition.CENTER}
    };

    private PanelPosition startPosition = PanelPosition.CENTER;
    private PanelPosition currentPosition = default;

    private PanelOpacity startOpacity = PanelOpacity.TRANSPARENT;
    private PanelOpacity currentOpacity = default;

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
        // DESIGN CHOICE: Load in styles that define the animations at runtime.
        // Why not just load it at compile time by specifying it in the UXML?
        // We can keep the logic/animations of PanelUI separate from its derivations.
        // For example, the Inventory shouldn't need to know about how it appears on the screen,
        // which is PanelUI's responsibility. 
        // The downside of loading on Awake is that this component is not guaranteed to be initialized
        // until Start, and so other scripts cannot use this until then
        StyleSheet styleSheet = Resources.Load<StyleSheet>(ANIMATIONS_USS_PATH);
        root.styleSheets.Add(styleSheet);

        // DESIGN CHOICE: Use a layer of abstraction between animation type and start position
        // to make it easier for designer to understand the animation that will be played

        // Initialize Panel tracking state
        startPosition = panelStartPosition[animationType];
        currentPosition = startPosition;

        startOpacity = (animationType == PanelAnimationType.NONE)? PanelOpacity.OPAQUE : PanelOpacity.TRANSPARENT;
        root.style.visibility = (animationType == PanelAnimationType.NONE)? Visibility.Visible : Visibility.Hidden;
        currentOpacity = startOpacity;

        // Initialize state of Panel UI
        root.AddToClassList(panelPositionClasses[startPosition]);
        root.AddToClassList(panelVisibilityClasses[startOpacity]);
        root.AddToClassList(animationType == PanelAnimationType.APPEAR? c_AnimationInstant : c_AnimationFast);
        
        // Set up animation callback
        if(panelOpenStateChanged != null)
            panelOpenStateChanged.OnEventRaised += HandlePanelOpenStateChanged;
        
        root.RegisterCallback<TransitionStartEvent>(HandleAnimationStart);
        root.RegisterCallback<TransitionEndEvent>(HandleAnimationEnd);
    }

    private void HandleAnimationEnd(TransitionEndEvent evt)
    {
        // DESIGN CHOICE: Break the animation abstraction layer between implementation and visuals
        // to explicitly turn off raycast blocking (via visibility toggle). Currently the only way to
        // disable raycasts is to literally set the visibility field to HIDDEN or display field to NONE.
        // Since display control is reserved for parent panels (such as tab UI), we use the visibility route.
        // The reason we must change the style here and not in the USS class is because the order in which 
        // the visibility field must be toggled relative to the opacity change varies based on whether it is
        // currently transparent or opaque from this script's perspective. As such, the USS tags for transparent and opaque
        // would need to include animation information. This would compromise the separation of responsibilities
        // for the USS classes and make it tricky to work with animations

        if(currentOpacity == PanelOpacity.TRANSPARENT)
        {
            root.style.visibility = Visibility.Hidden;   
        }
                  
    }

    private void HandleAnimationStart(TransitionStartEvent evt)
    {
        if(currentOpacity == PanelOpacity.OPAQUE)
        {
            root.style.visibility = Visibility.Visible;   
        }
    }

    void OnDestroy()
    {
        if(panelOpenStateChanged != null)
            panelOpenStateChanged.OnEventRaised -= HandlePanelOpenStateChanged;
        
        // No deregister because root has been destroyed at this point
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
        // Special case for if no animation should be played
        if(animationType == PanelAnimationType.NONE)
            return;

        ChangePosition(PanelPosition.CENTER);
        ChangeVisibility(PanelOpacity.OPAQUE);

        OnOpenPanel();
    }

    public void ClosePanel()
    {
        // Special case for if no animation should be played
        if(animationType == PanelAnimationType.NONE)
            return;

        ChangePosition(startPosition);
        ChangeVisibility(PanelOpacity.TRANSPARENT);

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

    private void ChangeVisibility(PanelOpacity visibility)
    {
        root.RemoveFromClassList(panelVisibilityClasses[currentOpacity]);
        root.AddToClassList(panelVisibilityClasses[visibility]);
        currentOpacity = visibility;
    }

}
