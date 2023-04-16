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

    // UI Visibility classes
    // DESIGN CHOICE: Keep visibility decoupled from position to make code more understandable and flexible
    const string c_Invisible = "invisible";
    const string c_Visible = "visible";

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
    public enum PanelVisibility {INVISIBLE, VISIBLE};
    private Dictionary<PanelVisibility, string> panelVisibilityClasses = new Dictionary<PanelVisibility, string>()
    {
        {PanelVisibility.INVISIBLE, c_Invisible},
        {PanelVisibility.VISIBLE, c_Visible}
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

    private PanelVisibility startVisibility = PanelVisibility.INVISIBLE;
    private PanelVisibility currentVisibility = default;

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

        startVisibility = (animationType == PanelAnimationType.NONE)? PanelVisibility.VISIBLE : PanelVisibility.INVISIBLE;
        currentVisibility = startVisibility;

        // Initialize state of Panel UI
        root.AddToClassList(panelPositionClasses[startPosition]);
        root.AddToClassList(panelVisibilityClasses[startVisibility]);
        root.AddToClassList(animationType == PanelAnimationType.APPEAR? c_AnimationInstant : c_AnimationFast);
        
        // Set up animation callback
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
        root.pickingMode = PickingMode.Position;

        // Special case for if no animation should be played
        if(animationType == PanelAnimationType.NONE)
            return;

        ChangePosition(PanelPosition.CENTER);
        ChangeVisibility(PanelVisibility.VISIBLE);

        OnOpenPanel();
    }

    public void ClosePanel()
    {
        root.pickingMode = PickingMode.Ignore;

        // Special case for if no animation should be played
        if(animationType == PanelAnimationType.NONE)
            return;

        ChangePosition(startPosition);
        ChangeVisibility(PanelVisibility.INVISIBLE);

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

    private void ChangeVisibility(PanelVisibility visibility)
    {
        root.RemoveFromClassList(panelVisibilityClasses[currentVisibility]);
        root.AddToClassList(panelVisibilityClasses[visibility]);
        currentVisibility = visibility;
    }

}
