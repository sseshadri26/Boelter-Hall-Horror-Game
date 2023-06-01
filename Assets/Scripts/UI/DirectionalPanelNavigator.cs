using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Navigates through the currently opened UI panel using directional input
/// </summary>
public class DirectionalPanelNavigator : MonoBehaviour
{
    [SerializeField] FirstPersonActionsSO input;
    [SerializeField] UIController panelController;
    Dictionary<GameObject, IDirectionControllable> panelToNavInterface = new Dictionary<GameObject, IDirectionControllable>();
    // TODO: On directional input, navigate through the currently opened panel

    void Awake()
    {
        input.controls.Movement.performed += HandleNavigation;
    }

    private void HandleNavigation(InputAction.CallbackContext context)
    {
        GameObject openPanel = panelController.GetOpenPanel();
        if (openPanel == null) return;

        if (!panelToNavInterface.ContainsKey(openPanel))
            panelToNavInterface[openPanel] = openPanel.GetComponent<IDirectionControllable>();

        if (panelToNavInterface[openPanel] == null) return;

        Vector2 movement = context.ReadValue<Vector2>();
        HandleHorizontalNavigation(movement.x, panelToNavInterface[openPanel]);
        HandleVerticalNavigation(movement.y, panelToNavInterface[openPanel]);
    }

    private void HandleVerticalNavigation(float y, IDirectionControllable navInterface)
    {
        const float THRESHOLD = 0.5f;
        if (y > THRESHOLD)
            navInterface.MoveUp();
        else if (y < -THRESHOLD)
            navInterface.MoveDown();
    }

    private void HandleHorizontalNavigation(float x, IDirectionControllable navInterface)
    {
        const float THRESHOLD = 0.5f;
        if (x > THRESHOLD)
            navInterface.MoveRight();
        else if (x < -THRESHOLD)
            navInterface.MoveLeft();
    }
}
