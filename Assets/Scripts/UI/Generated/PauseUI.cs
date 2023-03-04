using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Events;


public class PauseUI : MonoBehaviour
{
    [SerializeField] UIDocument document;
    [SerializeField] UnityEvent OnResumeButtonPressed;
    [SerializeField] UnityEvent OnRestartButtonPressed;
    [SerializeField] UnityEvent OnMainMenuButtonPressed;
    [SerializeField] UnityEvent OnSettingsButtonPressed;


    void Awake()
    {
        document.rootVisualElement.Query<Button>().Where((Button b) => b.parent.name == "ResumeButton").First().RegisterCallback<ClickEvent>(ev => OnResumeButtonPressed.Invoke());
        document.rootVisualElement.Query<Button>().Where((Button b) => b.parent.name == "RestartButton").First().RegisterCallback<ClickEvent>(ev => OnRestartButtonPressed.Invoke());
        document.rootVisualElement.Query<Button>().Where((Button b) => b.parent.name == "MainMenuButton").First().RegisterCallback<ClickEvent>(ev => OnMainMenuButtonPressed.Invoke());
        document.rootVisualElement.Query<Button>().Where((Button b) => b.parent.name == "SettingsButton").First().RegisterCallback<ClickEvent>(ev => OnSettingsButtonPressed.Invoke());

    }
}
