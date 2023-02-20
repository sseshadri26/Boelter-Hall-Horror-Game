using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;
using System.IO;


public class UIScriptGenerator
{
    // INSERTIONS
    const string INSERTIONTAG_NAME = "<NAME>";

    // FIELDS
    const string FIELD_UI_DOCUMENT = "[SerializeField] UIDocument document";

    // CONTENT TEMPLATES
    const string TEMPLATE_UNITY_EVENT = "[SerializeField] UnityEvent On<NAME>Pressed";
    const string TEMPLATE_QUERY = "document.rootVisualElement.Query<Button>().Where((Button b) => b.parent.name == \"<NAME>\").First().RegisterCallback<ClickEvent>(ev => On<NAME>Pressed.Invoke())";
    
    List<string> namespaceNames = new List<string>
    {
        "UnityEngine",
        "UnityEngine.UIElements",
        "UnityEngine.Events"
    };
    
    /// <summary>
    /// Create a script called "<paramref name="scriptName"/>.cs" at path <paramref name = "scriptFolderPath"/> 
    /// that fills in <paramref name = "scriptTemplate"/> 
    /// using the 
    /// </summary>
    public void GenerateScript(string scriptName, TextAsset scriptTemplate, VisualTreeAsset visualTreeAsset, string scriptFolderPath)
    {
        // Fill in root 
        VisualElement root = new VisualElement();
        TemplateContainer instance = visualTreeAsset.CloneTree();
        root.Add(instance);

        // Get list of all buttons in the doc
        List<string> buttonNames = root.Query<Button>().ForEach<string>((Button b) => b.parent.name);

        List<string> buttonEventStrings = buttonNames.Select(buttonName => GenerateStringByTemplate(TEMPLATE_UNITY_EVENT, buttonName)).ToList<string>();
        List<string> buttonQueryStrings = buttonNames.Select(buttonName => GenerateStringByTemplate(TEMPLATE_QUERY, buttonName)).ToList<string>();

        // Use new set of lists, since these may become concatenations of other lists
        List<string> externalStrings = new List<string>().Concat(namespaceNames.Select(namespaceName => "using " + namespaceName)).ToList();
        List<string> fieldsStrings = new List<string>().Append(FIELD_UI_DOCUMENT).Concat(buttonEventStrings).ToList();
        List<string> awakeStrings = new List<string>().Concat(buttonQueryStrings).ToList();

        List<ScriptGenerationUtils.ContentInsertionData> sections = new List<ScriptGenerationUtils.ContentInsertionData>
        {
            new ScriptGenerationUtils.ContentInsertionData{block = ScriptGenerationUtils.TemplateBlock.EXTERNAL, content = externalStrings},
            new ScriptGenerationUtils.ContentInsertionData{block = ScriptGenerationUtils.TemplateBlock.FIELDS, content = fieldsStrings},
            new ScriptGenerationUtils.ContentInsertionData{block = ScriptGenerationUtils.TemplateBlock.AWAKE, content = awakeStrings}
        };

        string generatedScript = ScriptGenerationUtils.GenerateScriptFromTemplate(scriptName, scriptTemplate.text, sections);

        File.WriteAllText(string.Format("{0}{1}.cs", scriptFolderPath, scriptName), generatedScript);
    }

    private string GenerateStringByTemplate(string template, string text) => template.Replace(INSERTIONTAG_NAME, text);
}

