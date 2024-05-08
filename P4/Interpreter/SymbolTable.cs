using System;
using System.Collections.Generic;

namespace P4.Interpreter;

public class SymbolTable
{
    private readonly Stack<Dictionary<string, object>> _symbolTable = new();

    public SymbolTable()
    {
        // Initialize with a global scope
        _symbolTable.Push(new Dictionary<string, object>());
    }

    public void Add(string name, object value)
    {
        _symbolTable.Peek().Add(name, value);
    }

    public object Get(string name)
    {
        foreach (var scope in _symbolTable)
        {
            if (scope.ContainsKey(name))
            {
                return scope[name];
            }
        }

        throw new Exception($"Variable {name} not found");
    }
    public bool IsVariableDeclared(string varName)
    {
        foreach (var scope in _symbolTable)
        {
            if (scope.ContainsKey(varName))
                return true;
        }
        return false;
    }
    
    public bool IsVariableDeclaredInScope(string varName)
    {
            if (_symbolTable.Peek().ContainsKey(varName))
                return true;
            return false;
    }
    public bool IsTypeCorrect(string varName, object value)
    {
        foreach (var scope in _symbolTable)
        {
            if (scope.ContainsKey(varName))
            {
                return scope[varName].GetType() == value.GetType();
            }
        }
        return false;
    }
    
    public void PushScope()
    {
        _symbolTable.Push(new Dictionary<string, object>());
    }

    public void PopScope()
    {
        _symbolTable.Pop();
    }
    
}