using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System;

public class EventChannelTesterTool : EditorWindow
{
    private string editorWindowUXMLPath => "Assets/UI Assets/Documents/EditorUXML/" + this.GetType().Name + ".uxml";
    private string editorWindowCSSPath => "Assets/UI Assets/Styles/" + this.GetType().Name + ".uss";
    // UI References
    VisualElement m_Root = default;
    Button m_BroadcastButton = default;
    ObjectField m_BoolChannelField = default;
    Toggle m_BoolValueField = default;

    // UI Tags
    const string k_BoolChannelField = "bool-channel-field";
    const string k_BoolValueField = "bool-val";
    const string k_BroadcastButton = "broadcast-button";



    [MenuItem("Window/Event Channel Tester Tool")]
    public static void OpenApplication()
    {
        EventChannelTesterTool wnd = GetWindow<EventChannelTesterTool>();
        wnd.titleContent = new GUIContent("Event Channel Tester Tool");
    }

    private void OnEnable()
    {
        m_Root = rootVisualElement;

        VisualTreeAsset asset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(editorWindowUXMLPath);
        TemplateContainer instance = asset.Instantiate();
        m_Root.Add(instance);

        StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(editorWindowCSSPath);
        m_Root.styleSheets.Add(styleSheet);

        m_BroadcastButton = m_Root.Q<Button>(k_BroadcastButton);
        m_BoolChannelField = m_Root.Q<ObjectField>(k_BoolChannelField);
        m_BoolValueField = m_Root.Q<Toggle>(k_BoolValueField);


        // Button Register
        m_BroadcastButton.RegisterCallback<ClickEvent>(HandleBroadcastButtonPressed);


        // Reload saved references
        m_BoolChannelField.value = EditorUtility.InstanceIDToObject(EditorPrefs.GetInt(k_BoolChannelField, -1));
        m_BoolValueField.value = EditorPrefs.GetBool(k_BoolValueField, false);
    }

    private void OnDisable()
    {
        if (m_BoolChannelField != null)
            EditorPrefs.SetInt(k_BoolChannelField, m_BoolChannelField.value.GetInstanceID());
        EditorPrefs.SetBool(k_BoolValueField, m_BoolValueField.value);
    }

    private void HandleBroadcastButtonPressed(ClickEvent evt)
    {
        (m_BoolChannelField.value as BoolEventChannelSO).RaiseEvent(m_BoolValueField.value);
    }
}
