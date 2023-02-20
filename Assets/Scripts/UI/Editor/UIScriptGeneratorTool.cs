using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using System;

public class UIScriptGeneratorTool : EditorWindow
{
    private UIScriptGenerator generator = default;
    private string editorWindowUXMLPath => "Assets/UI Assets/Documents/" + this.GetType().Name + ".uxml";
    private string editorWindowCSSPath => "Assets/UI Assets/Styles/" + this.GetType().Name + ".uss";

    private string GENERATION_FOLDER_PATH = "Assets/Scripts/UI/Generated/";
    private string scriptName = "DefaultUI";



    // UI References
    private VisualElement root = default;
    private BaseField<UnityEngine.Object> visualTreeAssetUXMLField = default;
    private BaseField<UnityEngine.Object> textAssetScriptTemplateField = default;
    private TextField scriptNameField = default;
    private Label scriptPathNameField = default;
    private Button generateScriptButton = default;

    


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
        visualTreeAssetUXMLField = root.Q<BaseField<UnityEngine.Object>>("FieldUXML");
        textAssetScriptTemplateField = root.Q<BaseField<UnityEngine.Object>>("FieldTemplate");
        scriptNameField = root.Q<TextField>("FieldScriptName");
        scriptPathNameField = root.Q<Label>("FieldGenerationPath");
        generateScriptButton = root.Q<Button>("GenerateScriptButton");

        
        scriptNameField.RegisterValueChangedCallback<string>(HandleScriptNameChanged);
        generateScriptButton.RegisterCallback<ClickEvent>(HandleGenerateButtonPressed);
    }

    private void HandleScriptNameChanged(ChangeEvent<string> evt)
    {
        scriptName = evt.newValue;
        scriptPathNameField.text = ScriptPathName;
    }

    private void HandleGenerateButtonPressed(ClickEvent evt)
    {
        generator.GenerateScript(
            scriptName, textAssetScriptTemplateField.value as TextAsset, 
            visualTreeAssetUXMLField.value as VisualTreeAsset, GENERATION_FOLDER_PATH);
        
        AssetDatabase.Refresh();
    }

    private string ScriptPathName => GENERATION_FOLDER_PATH + scriptName;
}
