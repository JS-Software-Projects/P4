using System;
using System.Collections.Generic;
using Antlr4.Runtime;

/*
namespace P4.Interpreter;

public class ScopeChecker : EduGrammarBaseVisitor<object>
{
    private readonly Stack<Dictionary<string, Type>> _scopes = new();

   
    public ScopeChecker()
    {
        // Initialize with a global scope
        _scopes.Push(new Dictionary<string, Type>());
    }

    // Override for ifBlock -> skal ændres til functionBlock når det er tilføjet til grammatikken.
    /*
    public override object VisitIfBlock(EduGrammarParser.IfBlockContext context)
    {
        _scopes.Push(new Dictionary<string, Type>()); // Enter new scope for the if block
        base.VisitIfBlock(context); // Visit children
        _scopes.Pop(); // Exit scope
        return null;
    }

    // Checking dupplicate declaration
    public override object VisitVariableDeclaration(EduGrammarParser.VariableDeclarationContext context)
    {
        var varName = context.id().GetText();
        if (IsVariableDeclared(varName))
        {
            throw new Exception($"Variable '{varName}' already declared.");
        }
        var varType = TypeChecker.GetTypeFromContext(context.type());
        _scopes.Peek().Add(varName, varType);
        return null;
    }
    
    // Checking if variable is declared
    public override object VisitAssignment(EduGrammarParser.AssignmentContext context)
    {
        var varName = context.id().GetText();
        if (!IsVariableDeclared(varName))
        {
            throw new Exception($"Variable '{varName}' not declared.");
        }
        return null;
    }

    private bool IsVariableDeclared(string varName)
    {
        return _scopes.Peek().ContainsKey(varName);
    }
}
*/
