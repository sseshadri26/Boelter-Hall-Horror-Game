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

    [Tooltip("How many seconds to wait before allowing another movement input")]
    [SerializeField] float moveSpamThreshold = 0.1f;
    float timeOfLastMove = 0;

    Dictionary<GameObject, IDirectionControllable> panelToNavInterface = new Dictionary<GameObject, IDirectionControllable>();

    void OnEnable()
    {
        input.controls.Movement.performed += HandleNavigation;
        input.controls.Submit.performed += HandleSubmit;
    }
    void OnDisable()
    {
        input.controls.Movement.performed -= HandleNavigation;
        input.controls.Submit.performed -= HandleSubmit;
    }

    private void HandleSubmit(InputAction.CallbackContext context)
    {
        GameObject openPanel = panelController.GetOpenPanel();
        if (openPanel == null) return;

        if (!panelToNavInterface.ContainsKey(openPanel))
            panelToNavInterface[openPanel] = openPanel.GetComponent<IDirectionControllable>();

        if (panelToNavInterface[openPanel] == null) return;

        panelToNavInterface[openPanel].Submit();
    }

    private void HandleNavigation(InputAction.CallbackContext context)
    {
        // Prevent rapid movement that might be too fast for player to keep up with
        // Use real time instead of normal time in case time scale has been set to 0
        if (Time.realtimeSinceStartup - timeOfLastMove < moveSpamThreshold) return;

        GameObject openPanel = panelController.GetOpenPanel();
        if (openPanel == null) return;

        if (!panelToNavInterface.ContainsKey(openPanel))
            panelToNavInterface[openPanel] = openPanel.GetComponent<IDirectionControllable>();

        if (panelToNavInterface[openPanel] == null) return;

        Vector2 movement = context.ReadValue<Vector2>();
        HandleHorizontalNavigation(movement.x, panelToNavInterface[openPanel]);
        HandleVerticalNavigation(movement.y, panelToNavInterface[openPanel]);

        timeOfLastMove = Time.realtimeSinceStartup;


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
