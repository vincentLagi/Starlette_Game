using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Derives from MonoBehaviour to keep the context in a specific Room
/// </summary>
public class CompilerContext : MonoBehaviour
{
    private Dictionary<string, VariableBlock> Variables = new();

    public List<VariableBlock> GetAllVariables()
    {
        return new List<VariableBlock>(Variables.Values);
    }
    public void DeclareAndAssignVariable(string name, VariableBlock value = null)
    {
        if (value == null)
        {
            DeclareVariable(name);
        }
        else
        {
            AssignVariable(name, value);
        }
    }
    public PayloadResultModel DeclareVariable(string name)
    {
        if (Variables.ContainsKey(name))
        {
            return new PayloadResultModel("Variable already declared.", false);
        }
        // add new entries to variables
        Variables.Add(name, null);
        return new PayloadResultModel("Variable declared successfully.", true);
    }
    public PayloadResultModel AssignVariable(string name, VariableBlock value)
    {

        if (!Variables.ContainsKey(name))
        {
            return new PayloadResultModel("Variable not declared.", false);
        }
        Variables[name] = value;
        return new PayloadResultModel("Variable assigned successfully.", true);
    }


    public VariableBlock GetVariable(string name)
    {
        if (!Variables.ContainsKey(name))
        {
            throw new KeyNotFoundException($"Variable '{name}' not found in the context.");
        }
        return Variables[name];
    }
}
