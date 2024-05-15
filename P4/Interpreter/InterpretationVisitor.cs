using System;
using P4.Interpreter.AST;
using P4.Interpreter.AST.Nodes;

namespace P4.Interpreter;

public class InterpretationVisitor : IASTVisitor<object>
{
    private Environment _environment = new();
    public object Visit(ASTNode node)
    {
        return node.Accept(this);
    }

    public object Visit(ProgramNode node)
    {
        foreach (var child in node.GetChildren())
        {
            Visit(child);
        }
        return null;
    }

    public object Visit(BlockStatement node)
    {
        
        foreach (var statement in node.Statements)
        {
            if (statement is ReturnStatement)
            {
                return Visit(statement);
            } 
            Visit(statement);
            
        }
        return null;
    }

    public object Visit(BinaryExpression node)
    {
        var left = Visit(node.Left);
        var right = Visit(node.Right);

        object result;
        switch (node.Operator)
        {
            case Operator.Add when left is double:
                result = (double)left + (double)right;
                break;
            case Operator.Add when left is string:
                result = (string)left + (string)right;
                break;
            case Operator.Subtract when left is double:
                result = (double)left - (double)right;
                break;
            case Operator.Multiply when left is double:
                result = (double)left * (double)right;
                break;
            case Operator.Divide when left is double && (double)right != 0.0:
                result = (double)left / (double)right;
                break;
            case Operator.Divide when left is double && (double)right == 0.0:
                throw new Exception("Division by zero");
            case Operator.LessThan when left is double:
                result = (double)left < (double)right;
                break;
            case Operator.LessThanOrEqual when left is double:
                result = (double)left <= (double)right;
                break;
            case Operator.GreaterThan when left is double:
                result = (double)left > (double)right;
                break;
            case Operator.GreaterThanOrEqual when left is double:
                result = (double)left >= (double)right;
                break;
            case Operator.Equal:
                result = left.ToString() == right.ToString();
                break;
            case Operator.NotEqual:
                result = left.ToString() != right.ToString();
                break;
            case Operator.And:
                result = (bool)left && (bool)right;
                break;
            case Operator.Or:
                result = (bool)left || (bool)right;
                break;
            default:
                throw new Exception("Unknown operator");
        }
        return result;
    }

    public object Visit(UnaryExpression node)
    {
        var value = Visit(node.Operand);

        object result;
        switch (node.Operator)
        {
            case Operator.Not:
                result = !(bool)value;
                break;
            case Operator.Subtract:
                result = -(double)value;
                break;
            default:
                throw new Exception("Unknown operator");
        }
        return result;
    }

    public object Visit(TernaryExpression node)
    {
        var value = Visit(node.Condition);
        return (bool)value ? Visit(node.ThenExpression) : Visit(node.ElseExpression);
    }

    public object Visit(AssignmentStatement node)
    {
        var value = Visit(node.Expression);
        
        _environment.Add(node.VariableName.Name, value);
        return null;
    }

    public object Visit(FunctionCallStatement node)
    {
        var function = (FunctionDeclaration)_environment.Get(node.FunctionName);
        var originalEnvironment = _environment; // Save the current environment
        _environment = function.GetEnvironment().Copy(); // Switch to the function's environment
        _environment.PushScope();

        for (int i = 0; i < node.Arguments.Count; i++)
        {
            var param = function.ParameterList.Parameters[i];
            var argValue = Visit(node.Arguments[i]);
            _environment.DeclareVariable(param.ParameterName.Name);
            _environment.Add(param.ParameterName.Name, argValue);
        }

        var result = Visit(function.Block); // Execute the function body in the new environment

        _environment.PopScope();
        _environment = originalEnvironment; // Restore the original environment
        return result;
    }

    public object Visit(VariableDeclaration node)
    {
        _environment.DeclareVariable(node.VariableName.Name);

        if (node.Expression == null)
        {
            // No expression, set default value based on type
            switch (node.Type.TypeName)
            {
                case "Num":
                    _environment.Add(node.VariableName.Name, 0.0);
                    break;
                case "String":
                    _environment.Add(node.VariableName.Name, " ");
                    break;
                case "Bool":
                    _environment.Add(node.VariableName.Name, false);
                    break;
                default:
                    // Handle unexpected type (optional)
                    throw new ArgumentException("Unsupported variable type: " + node.Type.TypeName);
            }
        }
        else
        {
            var value = Visit(node.Expression);
            _environment.Add(node.VariableName.Name, value);
        }

        return null;
    }
    
    
    /*
    public object Visit(VariableDeclaration node)
    {
        _environment.DeclareVariable(node.VariableName.Name);
        if (node.Expression != null)
        {
            var value = Visit(node.Expression);
            _environment.Add(node.VariableName.Name, value);
        }
        
        return null;
    }
    */

    public object Visit(ConstantExpression node)
    {
        return node.Value;
    }

    public object Visit(IdentifierExpression node)
    {
        return _environment.Get(node.Name);
    }
    
    

    public object Visit(ParameterNode node)
    {
        return Visit(node.ParameterName);
    }

    public object Visit(FunctionDeclaration node)
    {
        node.SetEnvironment(_environment);
        _environment.DeclareVariable(node.FunctionName.Name);
        _environment.Add(node.FunctionName.Name, node);
        return null;
    }

    public object Visit(PrintStatement node)
    {
        var valueToPrint = Visit(node.Expression);
        
        Console.WriteLine(valueToPrint);
        
        Terminal.AddMessage(false, valueToPrint.ToString());

        return null;
    }

    public object Visit(IfBlock node)
    {
        var condition = (bool)Visit(node.Condition);

        if (condition)
        {
            return Visit(node.Block);
        }
        else if (node.ElseBlock != null)
        {
            return Visit(node.ElseBlock);
        }

        return null;
    }

    public object Visit(WhileBlock node)
    {
        var condition = (bool)Visit(node.Condition);
        if(condition)
        {
            Visit(node.Block);
            return Visit(node);
        }
        return null;
    }

    public object Visit(ReturnStatement node)
    {
        var valueToReturn = Visit(node.Expression);
        
        return valueToReturn;
    }

    public object Visit(ForLoopStatement node)
    {
        Visit(node.Initialization);
        while ((bool)Visit(node.Condition))
        {
            Visit(node.Block);
            Visit(node.Increment);
        }
        return null;
    }
}