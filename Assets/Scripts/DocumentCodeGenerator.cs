using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;
using UnityEngine.Events;
using System;
using System.Text.RegularExpressions;
using System.IO;

public class DocumentCodeGenerator : MonoBehaviour
{
    [SerializeField] string scriptName = "NewScript";
    [SerializeField] UIDocument document;
    [SerializeField] TextAsset scriptTemplateFile;

    // STRINGS
    // DESIGN CHOICE: Include entire namespace so that it doesn't depend on the base
    // script template to include them

    // TAGS
    const string TAG_FIELDS = "#FIELDS#";
    const string TAG_AWAKE = "#AWAKE#";
    const string TAG_CLASS_NAME = "#CLASSNAME#";

    const string TAG_NAME = "#NAME#";


    // FIELDS
    const string FIELD_UI_DOCUMENT = "[SerializeField] UnityEngine.UIElements.UIDocument document";

    // CONTENT TEMPLATES
    const string TEMPLATE_UNITY_EVENT = "[SerializeField] UnityEngine.Events.UnityEvent On#NAME#Pressed";
    const string TEMPLATE_QUERY = "document.rootVisualElement.Query<Button>().Where((Button b) => b.parent.name == \"#NAME#\").First().RegisterCallback<ClickEvent>(ev => On#NAME#Pressed.Invoke())";
    
    
    // Start is called before the first frame update
    void Start()
    {
        
        // Get list of all buttons in the doc
        List<string> buttonNames = document.rootVisualElement.Query<Button>().ForEach<string>((Button b) => b.parent.name);

        List<string> buttonEventStrings = buttonNames.Select(buttonName => GenerateStringByTemplate(TEMPLATE_UNITY_EVENT, buttonName)).ToList<string>();
        List<string> buttonQueryStrings = buttonNames.Select(buttonName => GenerateStringByTemplate(TEMPLATE_QUERY, buttonName)).ToList<string>();

        // Use new set of lists, since these may become concatenations of other lists
        List<string> fieldsStrings = buttonEventStrings.Append(FIELD_UI_DOCUMENT).ToList();
        List<string> awakeStrings = buttonQueryStrings;

        List<Section> sections = new List<Section>
        {
            new Section{sectionTag = TAG_FIELDS, sectionContent = fieldsStrings},
            new Section{sectionTag = TAG_AWAKE, sectionContent = awakeStrings}
        };

        string generatedScript = GenerateScriptFromTemplate(scriptTemplateFile.text, scriptName, sections);

        File.WriteAllText(string.Format("Assets/Scripts/Generated/{0}.cs", scriptName), generatedScript);
    }

    private string GenerateStringByTemplate(string template, string text) => template.Replace(TAG_NAME, text);

    private string GenerateScriptFromTemplate(string scriptTemplate, string scriptName, List<Section> sections)
    {
        foreach(Section s in sections)
        {
            scriptTemplate = GetUpdatedTemplateWithContent(scriptTemplate, s.sectionTag, s.sectionContent);
        }

        scriptTemplate = scriptTemplate.Replace(TAG_CLASS_NAME, scriptName);
        return scriptTemplate;
    }

    // Fill in a specific tag with new content (takes into account indent of tag)
    private string GetUpdatedTemplateWithContent(string scriptTemplate, string tag, List<string> content)
    {
        Match m = Regex.Match(scriptTemplate, string.Format(" *{0}", tag));
        if(m.Success)
        {
            int indentSize = m.Value.Length - tag.Length;
            string section = content.Aggregate("", (accum, curElem) => accum + new String(' ', indentSize) + curElem + ";\n");
            return scriptTemplate.Replace(m.Value, section);
        }
        return scriptTemplate;
    }
}

class Section
{
    public string sectionTag;
    public List<string> sectionContent;
}