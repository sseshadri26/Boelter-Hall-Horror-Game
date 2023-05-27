using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class PauseUI : MonoBehaviour
{
    [SerializeField] UIDocument document = default;
    [SerializeField] UnityEvent OnRestartButtonPressed = new UnityEvent();
    [SerializeField] UnityEvent OnResumeButtonPressed = new UnityEvent();
    [SerializeField] UnityEvent OnMainMenuButtonPressed = new UnityEvent();
    [SerializeField] UnityEvent OnSettingsButtonPressed = new UnityEvent();


    const string k_RestartButton = "restart-button";
    const string k_ResumeButton = "resume-button";
    const string k_MainMenuButton = "main-menu-button";
    const string k_SettingsButton = "settings-button";

    
    Button m_RestartButton = default;
    Button m_ResumeButton = default;
    Button m_MainMenuButton = default;
    Button m_SettingsButton = default;

    VisualElement root
    {
        get {
            if(document != null)
                return document.rootVisualElement;
            return null;
        }
    }

    void Awake()
    {
        m_RestartButton = root.Q<Button>(k_RestartButton);
        m_ResumeButton = root.Q<Button>(k_ResumeButton);
        m_MainMenuButton = root.Q<Button>(k_MainMenuButton);
        m_SettingsButton = root.Q<Button>(k_SettingsButton);

        m_RestartButton.RegisterCallback<ClickEvent>(ev => OnRestartButtonPressed.Invoke());
        m_ResumeButton.RegisterCallback<ClickEvent>(ev => OnRestartButtonPressed.Invoke());
        m_MainMenuButton.RegisterCallback<ClickEvent>(ev => OnMainMenuButtonPressed.Invoke());
        m_SettingsButton.RegisterCallback<ClickEvent>(ev => OnSettingsButtonPressed.Invoke());
    }
}
