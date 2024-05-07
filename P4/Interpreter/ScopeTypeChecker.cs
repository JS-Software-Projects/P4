using System;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
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
        Type leftType = Visit(node.Left);
        Type rightType = Visit(node.Right);
        
        if (leftType != rightType)
        {
            throw new Exception("Type mismatch in binary expression.");
        }
        
        switch (node.Operator)
        {
            case Operator.Add:
                if(leftType.TypeName == "Num" || leftType.TypeName == "String")
                {
                    return leftType;
                }
                else
                {
                    throw new Exception("Type mismatch in binary expression: expected a Number or a String.");
                }
            case Operator.Subtract:
            case Operator.Multiply:
            case Operator.Divide:
                if (leftType.TypeName == "Num")
                {
                    return leftType;
                }
                else
                {
                    throw new Exception("Type mismatch in binary expression: expected a Number.");
                }
            case Operator.LessThan:
            case Operator.LessThanOrEqual:
            case Operator.GreaterThan:
            case Operator.GreaterThanOrEqual:
                if (leftType.TypeName == "Num")
                {
                    return new Type("Bool");
                }
                else
                {
                    throw new Exception("Type mismatch in binary expression: expected a Number.");
                }
            case Operator.Equal:
            case Operator.NotEqual:
                if (leftType==rightType)
                {
                    return new Type("Bool");
                }
                else
                {
                    throw new Exception("Type mismatch in binary expression: expected a Number.");
                }
               
            default:
                throw new Exception("Unknown binary operator.");
        }
    }

    public Type Visit(UnaryExpression node)
    {
        Type operandType = Visit(node.Operand);
        
        switch (node.Operator)
        {
            case Operator.Not:
                if (operandType.TypeName != "Bool")
                {
                    throw new Exception("Type mismatch in unary expression: expected Bool.");
                }
                return operandType;
            case Operator.Subtract:
                if (operandType.TypeName != "Num")
                {
                    throw new Exception("Type mismatch in unary expression: expected a Number.");
                }
                return operandType;
            default:
                throw new Exception("Unknown unary operator.");
        }
    }

    public Type Visit(TernaryExpression node)
    {
        
        Type conditionType = Visit(node.Condition);
        if (conditionType.TypeName != "Bool")
        {
            throw new Exception("Condition in ternary expression must be of type Bool.");
        }
        
        Type trueExpressionType = Visit(node.ThenExpression);
        Type falseExpressionType = Visit(node.ElseExpression);

        if (trueExpressionType.TypeName != falseExpressionType.TypeName)
        {
            throw new Exception("True and false expressions in a ternary operation must be of the same type.");
        }
        
        return trueExpressionType;
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
        
        var varType = new Type(node.Type);
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
        if (node.Value is int || node.Value is double)
        {
            return new Type("Num");
        }
        else if (node.Value is bool)
        {
            return new Type("Bool");
        }
        else if (node.Value is string)
        {
            return new Type("String");
        }
        else
        {
            throw new Exception("Unknown type in constant expression.");
        }
    }
    public Type Visit(IdentifierExpression node)
    {
        var varName = node.Name;
        if (!_symbolTableType.IsVariableDeclared(varName))
        {
            throw new Exception($"Variable '{varName}' not declared.");
        }

        
        return _symbolTableType.GetVariableType(varName);
    }

    public Type Visit(ParameterNode node)
    {
        throw new System.NotImplementedException();
    }

    public Type Visit(FunctionDeclaration node)
    {
        var blockType = Visit(node.Statements);
        var returnType = Visit(node.ReturnType);
        
        if (returnType != blockType)
        {
            throw new Exception("Return type mismatch.");
        }

        return null;
    }

    public Type Visit(PrintStatement node)
    {
        Visit(node.Expression);
        return null;
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
        return Visit(node.Expression);
    }

    public Type Visit(ForLoopStatement node)
    {
        throw new System.NotImplementedException();
    }
}