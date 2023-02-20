using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Linq;
using System;

public static class ScriptGenerationUtils
{
    // Data structure for holding information about new content
    // to insert into a template as well as the block it should be
    // inserted into
    public class ContentInsertionData
    {
        public TemplateBlock block;
        public List<string> content;
    }

    // BLOCKS -- tags representing where a block of code should be inserted
    public enum TemplateBlock
    {
        EXTERNAL,
        FIELDS,
        AWAKE
    }

    // DESIGN CHOICE: Leave this dictionary private, since other scripts shouldn't need
    // to know the tags that correspond to the blocks
    static Dictionary<TemplateBlock, string> TemplateBlockTags = new Dictionary<TemplateBlock, string>
    {
        {TemplateBlock.EXTERNAL, "#EXTERNAL#"},
        {TemplateBlock.FIELDS, "#FIELDS#"},
        {TemplateBlock.AWAKE, "#AWAKE#"}
    };

    // INSERTIONS -- tags representing an in-line replacement as opposed to a block of new lines like with a tag
    const string INSERTIONTAG_CLASS_NAME = "<CLASSNAME>";


    /// <summary>
    /// Generate the content of a script using the provided template and content (scriptName, sections).
    /// </summary>
    public static string GenerateScriptFromTemplate(string scriptName, string scriptTemplate, List<ContentInsertionData> data)
    {
        foreach(ContentInsertionData d in data)
        {
            scriptTemplate = GetUpdatedTemplateWithContent(scriptTemplate, d);
        }

        scriptTemplate = scriptTemplate.Replace(INSERTIONTAG_CLASS_NAME, scriptName);
        return scriptTemplate;
    }

    // Fill in a specific tag with new content (takes into account indent of tag)
    private static string GetUpdatedTemplateWithContent(string scriptTemplate, ContentInsertionData section)
    {
        Match m = Regex.Match(scriptTemplate, string.Format(" *{0}", TemplateBlockTags[section.block]));
        if(m.Success)
        {
            int indentSize = m.Value.Length - TemplateBlockTags[section.block].Length;
            string sectionStr = section.content.Aggregate("", (accum, curElem) => accum + new String(' ', indentSize) + curElem + ";\n");
            return scriptTemplate.Replace(m.Value, sectionStr);
        }
        return scriptTemplate;
    }
}
