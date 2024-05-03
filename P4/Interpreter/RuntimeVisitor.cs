using System;
using System.Collections.Generic;

namespace P4.Interpreter;

public class RuntimeVisitor : EduGrammarBaseVisitor<object?>
{
    /*
    private readonly Dictionary<string, object> _variables = new();
    private int _lineNumber = 0;
    
    public override object? VisitAssignment(EduGrammarParser.AssignmentContext context)
    {
        var variableName = context.id().GetText();
        var value = Visit(context.expr());
        Console.WriteLine($"{variableName} = {value}"+" is type "+value.GetType());
        _variables[variableName] = value;
        return null;
    }

    public override object? VisitIdentifier(EduGrammarParser.IdentifierContext context)
    {
        var variableName = context.id().GetText();
        if (!_variables.ContainsKey(variableName))
        {
            throw new Exception($"Variable '{variableName}' not found.");
        }
        return _variables[variableName];
    }
    */
}


