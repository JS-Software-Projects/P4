using System;
using System.Collections.Generic;
namespace P4.Interpreter;

public class VariableTable
{
    private readonly Dictionary<string, object> _variables = new();

    public void AddVariable(string name, object value)
    {
        _variables[name] = value;
    }

    public object GetVariableValue(string name)
    {
        if (!_variables.TryGetValue(name, out var value))
        {
            throw new ArgumentException($"Variable {name} does not exist.");
        }

        return value;
    }
}