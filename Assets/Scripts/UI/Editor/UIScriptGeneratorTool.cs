using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System;

public class UIScriptGeneratorTool : EditorWindow
{
    private UIScriptGenerator generator = default;
    private string editorWindowUXMLPath => "Assets/UI Assets/Documents/" + this.GetType().Name + ".uxml";
    private string editorWindowCSSPath => "Assets/UI Assets/Styles/" + this.GetType().Name + ".uss";

    private string GENERATION_FOLDER_PATH = "Assets/Scripts/UI/Generated/";



    // UI References
    private VisualElement root = default;
    private ObjectField visualTreeAssetUXMLField = default;
    private ObjectField textAssetScriptTemplateField = default;
    private TextField scriptNameField = default;
    private Label scriptPathNameField = default;
    private Button generateScriptButton = default;


    // DESIGN CHOICE: Not storing any data other than cached UI references since
    // this is purely a tool and not a UI for some object


    [MenuItem("Window/UI Script Generator Tool")]
    public static void OpenApplication()
    {
        UIScriptGeneratorTool wnd = GetWindow<UIScriptGeneratorTool>();
        wnd.titleContent = new GUIContent("UI Script Generator Tool");
    }

    private void OnEnable()
    {
        root = rootVisualElement;
        generator = new UIScriptGenerator();

        VisualTreeAsset original = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(editorWindowUXMLPath);
        TemplateContainer instance = original.CloneTree();
        root.Add(instance);

        StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(editorWindowCSSPath);
        root.styleSheets.Add(styleSheet);

        // Cache references
        visualTreeAssetUXMLField = root.Q<ObjectField>("FieldUXML");
        textAssetScriptTemplateField = root.Q<ObjectField>("FieldTemplate");
        scriptNameField = root.Q<TextField>("FieldScriptName");
        scriptPathNameField = root.Q<Label>("FieldGenerationPath");
        generateScriptButton = root.Q<Button>("GenerateScriptButton");

        // Restore values from before reload
        visualTreeAssetUXMLField.value = EditorUtility.InstanceIDToObject(EditorPrefs.GetInt("UIGEN_UXML", -1));
        textAssetScriptTemplateField.value = EditorUtility.InstanceIDToObject(EditorPrefs.GetInt("UIGEN_Template", -1));
        scriptNameField.value = EditorPrefs.GetString("UIGEN_ScriptName", "");

        scriptNameField.RegisterValueChangedCallback<string>(HandleScriptNameChanged);
        generateScriptButton.RegisterCallback<ClickEvent>(HandleGenerateButtonPressed);
    }

    private void OnDisable()
    {
        if(visualTreeAssetUXMLField.value != null)
            EditorPrefs.SetInt("UIGEN_UXML", visualTreeAssetUXMLField.value.GetInstanceID());
        if(textAssetScriptTemplateField.value != null)
            EditorPrefs.SetInt("UIGEN_Template", textAssetScriptTemplateField.value.GetInstanceID());
        EditorPrefs.SetString("UIGEN_ScriptName", scriptNameField.value);
    }

    private void HandleScriptNameChanged(ChangeEvent<string> evt)
    {
        scriptPathNameField.text = ScriptPathName;
    }

    private void HandleGenerateButtonPressed(ClickEvent evt)
    {
        generator.GenerateScript(
            scriptNameField.value, textAssetScriptTemplateField.value as TextAsset, 
            visualTreeAssetUXMLField.value as VisualTreeAsset, GENERATION_FOLDER_PATH);
        
        AssetDatabase.Refresh();
    }

    private string ScriptPathName => GENERATION_FOLDER_PATH + scriptNameField.value;
}
