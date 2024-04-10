using System;
using System.Collections.Generic;

namespace P4.Interpreter;

public class RuntimeVisitor : EduGrammarBaseVisitor<object?>
{
    private readonly Dictionary<string, object> _variables = new();
    private int _lineNumber = 0;
    
    public override object? VisitAssignment(EduGrammarParser.AssignmentContext context)
    {
        var variableName = context.id().GetText();
        var value = Visit(context.expr()); // Evaluate the expression

        // Find the closest scope where variable is declared (if your design supports shadowing, for example)
        foreach (var scope in scopes)
        {
            if (scope.ContainsKey(variableName))
            {
                // Here, you could add type checking based on the type stored in the scope.
                // For simplicity, let's assume _variables is a runtime environment separate from scopes.
                _variables[variableName] = value;
                return null;
            }
        }

        throw new Exception($"Variable '{variableName}' not declared."); // Should not reach here if scope checking is done prior
    }

    public override object? VisitIdExpr(EduGrammarParser.IdExprContext context)
    {
        var variableName = context.id().GetText();
        if (!_variables.ContainsKey(variableName))
        {
            throw new Exception($"Variable '{variableName}' not found.");
        }
        return _variables[variableName];
    }
}


