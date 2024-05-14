using System;
using System.Collections.Generic;

namespace P4.Interpreter;

public class SymbolTable<TKey,TValue>
{
    private Stack<Dictionary<TKey, TValue>> _symbolTable = new();

    public SymbolTable()
    {
        // Initialize with a global scope
        _symbolTable.Push(new Dictionary<TKey, TValue>());
    }
    // Copy constructor
    private SymbolTable(Stack<Dictionary<TKey, TValue>> symbolTable)
    {
        _symbolTable = new Stack<Dictionary<TKey, TValue>>(symbolTable);
    }

    // Copy method
    public SymbolTable<TKey, TValue> Copy()
    {
        var tempStack = new Stack<Dictionary<TKey, TValue>>();

        // Deep copy each scope (dictionary)
        foreach (var scope in _symbolTable)
        {
            var newScope = new Dictionary<TKey, TValue>(scope);
            tempStack.Push(newScope);
        }

        // Reverse the stack to maintain the original order
        var reversedStack = new Stack<Dictionary<TKey, TValue>>();
        while (tempStack.Count > 0)
        {
            reversedStack.Push(tempStack.Pop());
        }

        // Use the copy constructor to create a new SymbolTable with the reversed stack
        return new SymbolTable<TKey, TValue>(reversedStack);
    }
    

    public void Add(TKey name, TValue value)
    {
        _symbolTable.Peek().Add(name, value);
    }

    public TValue Get(TKey name)
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
    public object Set(TKey name, TValue value)
    {
        foreach (var scope in _symbolTable)
        {
            if (scope.ContainsKey(name))
            {
                scope[name] = value;
                return null;
            }
        }

        throw new Exception($"Variable {name} not found");
    }
    public bool IsVariableDeclared(TKey varName)
    {
        foreach (var scope in _symbolTable)
        {
            if (scope.ContainsKey(varName))
                return true;
        }
        return false;
    }
    
    public bool IsVariableDeclaredInScope(TKey varName)
    {
            if (_symbolTable.Peek().ContainsKey(varName))
                return true;
            return false;
    }
    public bool IsTypeCorrect(TKey varName, TValue value)
    {
        foreach (var scope in _symbolTable)
        {
            if (scope.ContainsKey(varName))
            {
                return scope[varName].Equals(value);
            }
        }
        return false;
    }
    
    public void PushScope()
    {
        _symbolTable.Push(new Dictionary<TKey, TValue>());
    }

    public void PopScope()
    {
        _symbolTable.Pop();
    }
    
}