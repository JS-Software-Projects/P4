using System;
using System.Collections.Generic;
using Antlr4.Runtime;

namespace P4.Interpreter;

public class ScopeChecker : EduGrammarBaseVisitor<object>
{
    private readonly Stack<Dictionary<string, Type>> _scopes = new();

   
    public ScopeChecker()
    {
        // Initialize with a global scope
        _scopes.Push(new Dictionary<string, Type>());
    }

    // Override for ifBlock
    public override object VisitIfBlock(EduGrammarParser.IfBlockContext context)
    {
        _scopes.Push(new Dictionary<string, Type>()); // Enter new scope for the if block
        base.VisitIfBlock(context); // Visit children
        _scopes.Pop(); // Exit scope
        return null;
    }

    // Override for elseBlock
    public override object VisitElseBlock(EduGrammarParser.ElseBlockContext context)
    {
        _scopes.Push(new Dictionary<string, Type>()); // Enter new scope for the else block
        base.VisitElseBlock(context); // Visit children
        _scopes.Pop(); // Exit scope
        return null;
    }

    // Override for whileBlock
    public override object VisitWhileBlock(EduGrammarParser.WhileBlockContext context)
    {
        _scopes.Push(new Dictionary<string, Type>()); // Enter new scope for the while block
        base.VisitWhileBlock(context); // Visit children
        _scopes.Pop(); // Exit scope
        return null;
    }
    
    // Checking dupplicate declaration
    public override object VisitDeclaration(EduGrammarParser.DeclarationContext context)
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
        foreach (var scope in _scopes)
        {
            if (scope.ContainsKey(varName))
            {
                return true;
            }
        }
        return false;
    }
}


