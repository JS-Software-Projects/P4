using System;
using System.Collections.Generic;
using P4.Interpreter.AST;

namespace P4.Interpreter;

public class ScopeTypeChecker : IASTVisitor<Type>
{
    private SymbolTable _symbolTableType = new();

    public Type Visit(ASTNode node)
    {
        throw new System.NotImplementedException();
    }

    public Type Visit(BlockStatement node)
    {
        foreach (var statement in node.Statements)
        {
            Visit(statement);
        }
        return null;
    }

    public Type Visit(BinaryExpression node)
    {
        throw new System.NotImplementedException();
    }

    public Type Visit(UnaryExpression node)
    {
        throw new System.NotImplementedException();
    }

    public Type Visit(TernaryExpression node)
    {
        throw new System.NotImplementedException();
    }

    
    public Type Visit(AssignmentStatement node)
    {
        // Scope rule
        var varName = node.VariableName;
        if (!_symbolTableType.IsVariableDeclared(varName))
        {
            throw new Exception("Variable not declared.");
        }
        //Type checking
        Type expression = Visit(node.Expression);
        
        if (!_symbolTableType.IsTypeCorrect(varName, expression))
        {
            throw new Exception("Type mismatch.");
        }
        return null;
    }

    public Type Visit(FunctionCallStatement node)
    {
        throw new System.NotImplementedException();
    }

    
    public Type Visit(VariableDeclaration node)
    {
        // Scope rule 
        var varName = node.VariableName;
        if (_symbolTableType.IsVariableDeclaredInScope(varName))
        {
            throw new Exception($"Variable '{varName}' already declared.");
        }

        // Type checking
        var expressionType = Visit(node.Expression);
        if (_symbolTableType.IsTypeCorrect(varName, expressionType))
        {
            throw new Exception($"Type mismatch for variable '{varName}'.");
        }
        
        var varType = node.Type;
        _symbolTableType.Add(varName, varType);
        return null;
    }
   

    public Type Visit(Expression node)
    {
        throw new System.NotImplementedException();
    }

    public Type Visit(Statement node)
    {
        throw new System.NotImplementedException();
    }

    public Type Visit(ConstantExpression node)
    {
        throw new System.NotImplementedException();
    }

    public Type Visit(IdentifierExpression node)
    {
        throw new System.NotImplementedException();
    }

    public Type Visit(ParameterNode node)
    {
        throw new System.NotImplementedException();
    }

    public Type Visit(FunctionDeclaration node)
    {
        throw new System.NotImplementedException();
    }

    public Type Visit(PrintStatement node)
    {
        throw new System.NotImplementedException();
    }

    public Type Visit(IfBlock node)
    {
        _symbolTableType.PushScope(); // Enter new scope for the if block
        Visit(node.Block); // Visit children
        _symbolTableType.PopScope(); // Exit scope
        return null;
    }

    public Type Visit(WhileBlock node)
    {
        throw new System.NotImplementedException();
    }

    public Type Visit(ReturnStatement node)
    {
        throw new System.NotImplementedException();
    }

    public Type Visit(ForLoopStatement node)
    {
        throw new System.NotImplementedException();
    }
}