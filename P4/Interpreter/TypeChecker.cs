using System;
using System.Collections.Generic;
using Antlr4.Runtime;

namespace P4.Interpreter;

public class TypeChecker : EduGrammarBaseVisitor<Type>
{
    private readonly Dictionary<string, Type> _types = new();
    
   public static Type GetTypeFromContext(ParserRuleContext context) {
    var text = context.GetText();

    return text switch
    {
        "Num" => typeof(double),
        "String" => typeof(string),
        "Bool" => typeof(bool),
        _ => throw new NotImplementedException($"Unsupported type: {text}")
    };
}

    public override Type VisitAssignment(EduGrammarParser.AssignmentContext context)
    {
        var variableName = context.id().GetText();
        var variableType = Visit(context.expr());
        if (_types.ContainsKey(variableName))
        {
            if (_types[variableName] != variableType)
            {
                throw new Exception($"Type mismatch for variable '{variableName}'. Expected {_types[variableName]}, but found {variableType}.");
            }
        }
        else
        {
            _types.Add(variableName, variableType);
        }
        return variableType;
    }

    public override Type VisitDeclaration(EduGrammarParser.DeclarationContext context)
    {
            var exprType = Visit(context.expr());
            var variableName = context.id().GetText();
            var variableType = GetTypeFromContext(context.type());
            if (exprType != variableType)
            {
                throw new Exception($"Type mismatch in declaration. Expected {variableType}, but found {exprType}.");
            }
            if (_types.ContainsKey(variableName))
            {
                throw new Exception($"Variable '{variableName}' already declared.");
            }

            _types.Add(variableName, variableType);
            return variableType;
       }
    public override Type VisitConstant(EduGrammarParser.ConstantContext context)
    {
        if (context.Num() != null) return typeof(double);
        if (context.String() != null) return typeof(string); // removes the quotes "" 
        if (context.Bool() != null) return typeof(bool);
        if (context.Null() != null) return null;
        throw new Exception("Invalid type for constant.");
    }
}