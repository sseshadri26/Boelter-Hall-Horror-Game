using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class PauseUI : MonoBehaviour, IDirectionControllable
{
    [SerializeField] UIDocument document = default;
    [SerializeField] UnityEvent OnRestartButtonPressed = new UnityEvent();
    [SerializeField] UnityEvent OnResumeButtonPressed = new UnityEvent();
    [SerializeField] UnityEvent OnMainMenuButtonPressed = new UnityEvent();
    [SerializeField] UnityEvent OnSettingsButtonPressed = new UnityEvent();


    const string k_ButtonsContainer = "ButtonsBox";
    const string k_RestartButton = "restart-button";
    const string k_ResumeButton = "resume-button";
    const string k_MainMenuButton = "main-menu-button";
    const string k_SettingsButton = "settings-button";


    // USS Classes
    const string c_ActivateButton = "activated";


    VisualElement m_ButtonsContainer = default;
    Button m_RestartButton = default;
    Button m_ResumeButton = default;
    Button m_MainMenuButton = default;
    Button m_SettingsButton = default;

    ItemSelector<MouseOverEvent> selector;

    VisualElement root
    {
        get
        {
            if (document != null)
                return document.rootVisualElement;
            return null;
        }
    }

    void Awake()
    {
        m_ButtonsContainer = root.Q<VisualElement>(k_ButtonsContainer);

        // Query from container to verify they are within the container
        m_RestartButton = m_ButtonsContainer.Q<Button>(k_RestartButton);
        m_ResumeButton = m_ButtonsContainer.Q<Button>(k_ResumeButton);
        m_MainMenuButton = m_ButtonsContainer.Q<Button>(k_MainMenuButton);
        m_SettingsButton = m_ButtonsContainer.Q<Button>(k_SettingsButton);

        selector = new ItemSelector<MouseOverEvent>(m_ButtonsContainer);

        m_RestartButton.RegisterCallback<ClickEvent>(ev => OnRestartButtonPressed.Invoke());
        m_ResumeButton.RegisterCallback<ClickEvent>(ev => OnResumeButtonPressed.Invoke());
        m_MainMenuButton.RegisterCallback<ClickEvent>(ev => OnMainMenuButtonPressed.Invoke());
        m_SettingsButton.RegisterCallback<ClickEvent>(ev => OnSettingsButtonPressed.Invoke());
    }

    public void Reset()
    {
        selector.Reset();
    }

    public void MoveUp()
    {
        selector.VisuallySelectPrev();
    }

    public void MoveDown()
    {
        selector.VisuallySelectNext();
    }

    public void MoveLeft() { }

    public void MoveRight() { }
}
