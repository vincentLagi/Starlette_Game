using System;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;



public class VariableBlockAdapter : VariableBlock
{
    [SerializeField] private TMP_InputField variableNameText;
    [SerializeField] private TMP_Dropdown dropdown;

    protected override void AdditionalAwake()
    {
        Value = gameObject.AddComponent<LiteralBlock>();
        Value.Init(DataType.CreateDataType<int>()); 

        if (variableNameText == null) variableNameText = GetComponentInChildren<TMP_InputField>();
        if(dropdown == null) dropdown = GetComponentInChildren<TMP_Dropdown>();
        
        if (variableNameText == null || dropdown == null)
        {
            Debug.LogError("VariableBlockAdapter: Missing TMP_InputField or TMP_Dropdown component.");
            return;
        }
    }
    public void TextFieldChanged()
    {
        VariableName = variableNameText.text;
        //Debug.Log($"Variable name changed to: {VariableName}");
    }

    public void DropdownOptionChanged()
    {
        // Get the selected option's text from the dropdown
        string selectedType = dropdown.options[dropdown.value].text;
        DataType type;
        if (selectedType == "Integer")
        {
            type = DataType.CreateDataType<int>();
        }
        else if (selectedType == "Float")
        {
            type = DataType.CreateDataType<float>();
        }
        else if (selectedType == "Boolean")
        {
            type = DataType.CreateDataType<bool>();
        }
        else
        {
            Debug.LogError("No Valid Option");
            return;
        }
        Value.SetValue(type);
        Debug.Log($"Variable type changed to: {selectedType}");
    }



    private static readonly string[] ReservedKeywords =
    {
        "auto", "break", "case", "char", "const",
        "continue", "default", "do", "double", "else",
        "enum", "extern", "float", "for", "goto",
        "if", "inline", "int", "long", "register",
        "restrict", "return", "short", "signed", "sizeof",
        "static", "struct", "switch", "typedef", "union",
        "unsigned", "void", "volatile", "while", "_Alignas",
        "_Alignof", "_Atomic", "_Bool", "_Complex", "_Generic",
        "_Imaginary", "_Noreturn", "_Static_assert", "_Thread_local"
    };  

    public static PayloadResultModel IsValidVariableName(string name)
    {
        if (string.IsNullOrEmpty(name))
            return new PayloadResultModel("Variable name cannot be empty.", false);

        // Check if it's a reserved keyword
        foreach (var keyword in ReservedKeywords)
        {
            if (name == keyword)
                return new PayloadResultModel($"Variable name cannot be a reserved keyword: {keyword}", false);
        }

        // Regular expression to match valid C# variable names
        // ^[a-zA-Z_][a-zA-Z0-9_]*$
        if (!Regex.IsMatch(name, @"^[a-zA-Z_][a-zA-Z0-9_]*$"))
        {
            return new PayloadResultModel($"Variable name '{name}' is not valid. Please re-check the Naming Convention Manual.", false);
        }

        return new PayloadResultModel("Variable name is valid.", true);
    }
}