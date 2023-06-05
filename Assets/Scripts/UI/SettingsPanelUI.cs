using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Events;

public class SettingsPanelUI : MonoBehaviour, IDirectionControllable
{
    // TODO
    // 1. Be able to select a setting to change
    // 2. Be able to adjust the setting when it is selected
    // 3. Needs a close button

    [SerializeField] UIDocument document;
    [SerializeField] UnityEvent onCloseButtonClicked;
    const string k_confirmButton = "confirm-button";

    Button m_closeButton;

    void Awake()
    {
        m_closeButton = document.rootVisualElement.Q<Button>(k_confirmButton);

        m_closeButton.RegisterCallback<ClickEvent>(ev => onCloseButtonClicked?.Invoke());
    }

    public void MoveDown()
    {
        // TODO: Select next setting
    }

    public void MoveLeft()
    {
        // TODO: Edit current setting left
    }

    public void MoveRight()
    {
        // TODO: Edit current setting right
    }

    public void MoveUp()
    {
        // TODO: Select prev setting
    }

    public void Submit()
    {
        // TODO: Confirm choices?
        onCloseButtonClicked?.Invoke();
    }

}
