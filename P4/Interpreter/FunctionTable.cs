using System;
using System.Collections.Generic;
namespace P4.Interpreter.Tests;

public class FunctionTable
{
    private readonly Dictionary<string, FunctionDeclaration> _functions = new();

    public void AddFunction(string name, FunctionDeclaration function)
    {
        if (_functions.ContainsKey(name))
        {
            throw new ArgumentException($"Function {name} already exists.");
        }

        _functions[name] = function;
    }

    public FunctionDeclaration GetFunction(string name)
    {
        if (!_functions.TryGetValue(name, out var function))
        {
            throw new ArgumentException($"Function {name} does not exist.");
        }

        return function;
    }
}