using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows.Forms.VisualStyles;
using P4.Interpreter.AST;
using P4.Interpreter.Tests;

namespace P4.Interpreter;

public class RunTimeInterpretor : IASTVisitor<object>
{
    private readonly VariableTable _variableTable = new();
    private readonly FunctionTable _functionTable = new();
    public object Visit(ASTNode node)
    {
        return node.Accept(this);
    }

    public object Visit(VariableDeclaration node)
    {
        
        var initialValue = node.Expression != null ? Visit(node.Expression) : null;
        _variableTable.AddVariable(node.VariableName.Name, initialValue);

        return null;
    }

    public object Visit(ConstantExpression node)
    {
        return node.Value;
    }

    public object Visit(IdentifierExpression node)
    {
        return _variableTable.GetVariableValue(node.Name);
    }

    public object Visit(ParameterNode node)
    {
        return Visit(node.ParameterName);
    }

    public object Visit(FunctionDeclaration node)
    {

        var function = new FunctionDeclaration(node.ReturnType, node.FunctionName, node.ParameterList, node.Statements);
        
        _functionTable.AddFunction(node.FunctionName.Name, function);

        return null;
    }

    public object Visit(PrintStatement node)
    {
        var valueToPrint = Visit(node.Expression);
        
        Console.WriteLine(valueToPrint);

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

    public object Visit(AssignmentStatement node)
    {
        var value = Visit(node.Expression);
        
        _variableTable.AddVariable(node.VariableName.Name, value);
        return null;
    }

    public object Visit(FunctionCallStatement node)
    {
        // Look up the function in the function table
        var function = _functionTable.GetFunction(node.FunctionName);

        // Evaluate the arguments
        var evaluatedArguments = node.Arguments.Select(arg => Visit(arg)).ToList();

        // Call the function with the evaluated arguments
        var result = function.Invoke(evaluatedArguments);

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
            Operator.Subtract => -(int)value,
            _ => throw new Exception("Unknown operator")
        };
    }
    
    public object Visit(TernaryExpression node)
    {
        var value = Visit(node.Condition);
        return (bool)value ? Visit(node.ThenExpression) : Visit(node.ElseExpression);
    }
  
    
}