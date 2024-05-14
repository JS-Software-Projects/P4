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
        object result = null;
        foreach (var statement in node.Statements)
        {
            result = Visit(statement);
        }
        return result;
    }

    public object Visit(BinaryExpression node)
    {
        var left = Visit(node.Left);
        var right = Visit(node.Right);

        return node.Operator switch
        {
            Operator.Add when left is double => (double)left + (double)right,
            Operator.Add when left is string => (string)left + (string)right,
            Operator.Subtract when left is double => (double)left - (double)right,
            Operator.Multiply when left is double => (double)left * (double)right,
            Operator.Divide when left is double && (double)right != 0.0 => (double)left / (double)right,
            Operator.Divide when left is double && (double)right == 0.0 => throw new Exception("Division by zero"),
            Operator.LessThan when left is double => (double)left < (double)right,
            Operator.LessThanOrEqual when left is double => (double)left <= (double)right,
            Operator.GreaterThan when left is double => (double)left > (double)right,
            Operator.GreaterThanOrEqual when left is double => (double)left >= (double)right,
            Operator.Equal => left == right,
            Operator.NotEqual => left != right,
            Operator.And => (bool)left && (bool)right,
            Operator.Or => (bool)left || (bool)right,
            _ => throw new Exception("Unknown operator")
        };
    }

    public object Visit(UnaryExpression node)
    {
        var value = Visit(node.Operand);

        return node.Operator switch
        {
            Operator.Not => !(bool)value,
            Operator.Subtract => -(double)value,
            _ => throw new Exception("Unknown operator")
        };
    }

    public object Visit(TernaryExpression node)
    {
        var value = Visit(node.Condition);
        return (bool)value ? Visit(node.ThenExpression) : Visit(node.ElseExpression);
    }

    public object Visit(AssignmentStatement node)
    {
        var value = Visit(node.Expression);
        
        _environment.Set(node.VariableName.Name, value);
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
        Console.WriteLine("should return result succes");
        return result;
    }

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

    public object Visit(ConstantExpression node)
    {
        return node.Value;
    }

    public object Visit(IdentifierExpression node)
    {
        return node.Name;
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