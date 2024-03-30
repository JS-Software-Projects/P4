using System;
using System.Collections.Generic;

namespace P4.Interpreter;

public class EduVisitor : EduGrammarBaseVisitor<object?>
{
    private readonly Dictionary<string, object> _variables = new();

    // Method to add a variable for testing
    public void AddVariableForTesting(string name, object value)
    {
        _variables[name] = value;
    }
    public override object? VisitAssignment(EduGrammarParser.AssignmentContext context)
    {
        var variableName = context.id().GetText();
        var value = Visit(context.expr());
        _variables[variableName] = value;
        return null;
    }
    //Needs to be tested.
    public override object? VisitConstant(EduGrammarParser.ConstantContext context)
    {
        if (context.Num() != null) return double.Parse(context.Num().GetText());
        if (context.String() != null) return context.String().GetText()[1..^1]; // removes the quotes "" 
        if (context.Bool() != null) return bool.Parse(context.Bool().GetText());
        if (context.Null() != null) return null;
        throw new Exception("Invalid constant");
    }
    //Has been tested.
    public override object? VisitIdExpr(EduGrammarParser.IdExprContext context)
    {
        var variableName = context.id().GetText();
        if (!_variables.ContainsKey(variableName)) throw new Exception($"Variable {variableName} not found");
        return _variables[variableName];
    }
    
    //Needs to be tested.
    public override object? VisitAddSubExpr(EduGrammarParser.AddSubExprContext context)
    {
        var left = Visit(context.expr(0));
        var right = Visit(context.expr(1));
        return context.addSubOp().start.Type switch
        {
            EduGrammarParser.ADD => Add(left, right),
            EduGrammarParser.SUB => Sub(left, right),
            _ => throw new Exception("Invalid operator")
        };
    }
    private object Add(object left, object right)
    {
        if (left is string || right is string) 
            return left.ToString()+ right;
        
        if (left is double l && right is double r)
            return l + r;
        
        throw new Exception($"Invalid addition. Cannot add {left?.GetType()} and {right?.GetType()}");
    }
    private object Sub(object left, object right)
    {
        if (left is double && right is double) return (double)left - (double)right;
        throw new Exception("Invalid subtraction");
    }
}