using System;
using System.Collections.Generic;
using Antlr4.Runtime;

namespace P4.Interpreter;

public class ScopeChecker : EduGrammarBaseVisitor<object>
{
    private Stack<Dictionary<string, VariableInfo>> scopes = new Stack<Dictionary<string, VariableInfo>>();

    public ScopeChecker()
    {
        // Initialize with a global scope
        scopes.Push(new Dictionary<string, VariableInfo>());
    }

    // Override for ifBlock
    public override object VisitIfBlock(EduGrammarParser.IfBlockContext context)
    {
        scopes.Push(new Dictionary<string, VariableInfo>()); // Enter new scope for the if block
        base.VisitIfBlock(context); // Visit children
        scopes.Pop(); // Exit scope
        return null;
    }

    // Override for elseBlock
    public override object VisitElseBlock(EduGrammarParser.ElseBlockContext context)
    {
        scopes.Push(new Dictionary<string, VariableInfo>()); // Enter new scope for the else block
        base.VisitElseBlock(context); // Visit children
        scopes.Pop(); // Exit scope
        return null;
    }

    // Override for whileBlock
    public override object VisitWhileBlock(EduGrammarParser.WhileBlockContext context)
    {
        scopes.Push(new Dictionary<string, VariableInfo>()); // Enter new scope for the while block
        base.VisitWhileBlock(context); // Visit children
        scopes.Pop(); // Exit scope
        return null;
    }
    
    // Override for a variable declaration node
    public override object VisitVarDecl(EduGrammarParser.VarDeclContext context)
    {
        string varName = context.varName.Text;
        if (IsVariableDeclared(varName))
        {
            throw new Exception($"Variable '{varName}' already declared.");
        }
        Type varType = GetTypeFromContext(context.type());
        scopes.Peek().Add(varName, new VariableInfo(varType, null));
        return null;
    }
    
    // Override for a variable reference node
    public override object VisitVarRef(EduGrammarParser.VarRefContext context)
    {
        string varName = context.varName.Text;
        if (!IsVariableDeclared(varName))
        {
            throw new Exception($"Variable '{varName}' not declared.");
        }
        return null;
    }

    private bool IsVariableDeclared(string varName)
    {
        foreach (var scope in scopes)
        {
            if (scope.ContainsKey(varName))
            {
                return true;
            }
        }
        return false;
    }
    
}

public class VariableInfo
{
    public Type Type { get; set; }
    public object Value { get; set; }

    public VariableInfo(Type type, object value)
    {
        Type = type;
        Value = value;
    }
}

