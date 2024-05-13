using System;
using P4.Interpreter.AST;

namespace P4.Interpreter.AST.Nodes;

//Abstract base class for all expressions
public abstract class Expression : ASTNode { }

public class IdentifierExpression : Expression
{
    public string Name { get; set; }
    
    public IdentifierExpression(string name)
    {
        Name = name;
    }
    public override T Accept<T>(IASTVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }

    public override string ToString()
    {
        return $"IdentifierExpression: Name is = {Name}";
    }
}

public class ConstantExpression : Expression
{
    public object Value { get; set; }
    public override T Accept<T>(IASTVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }

    public ConstantExpression(object value)
    {
        Value = value;
    }
    public override string ToString()
    {
        return $"ConstantExpression: is = {Value}";
    }
}

public class BinaryExpression : Expression
{
    public Expression Left { get; set; }
    public Operator Operator { get; set; } // Use an enum for operators
    public Expression Right { get; set; }
    public override T Accept<T>(IASTVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
    public BinaryExpression(Expression left, Operator op, Expression right)
    {
        Left = left;
        Operator = op;
        Right = right;
    }

    public override string ToString()
    {
        var leftExpr = Left != null ? Left.ToString() : "null";
        var rightExpr = Right != null ? Right.ToString() : "null";
        return $"({leftExpr} {Operator} {rightExpr})";
    
    }
    
}

public class UnaryExpression : Expression
{
    public Operator Operator { get; set; } // Use an enum for operators
    public Expression Operand { get; set; }

    public UnaryExpression(Operator op, Expression operand)
    {
        Operator = op;
        Operand = operand;
    }

    public override string ToString()
    {
        return $"UnaryExpression: Operand is = {Operand}. " +
               $"Operator is = {Operator}";
    }

    public override T Accept<T>(IASTVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
    
    
}

public class TernaryExpression : Expression
{
    public Expression Condition { get; set; }
    public Expression ThenExpression { get; set; }
    public Expression ElseExpression { get; set; }
    public override T Accept<T>(IASTVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }

    public TernaryExpression(Expression condition, Expression thenExpression, Expression elseExpression)
    {
        Condition = condition;
        ThenExpression = thenExpression;
        ElseExpression = elseExpression;
    }

    public override string ToString()
    {
        return $"UnaryExpression: Condition is = {Condition}" +
               $"thenExpression is = {ThenExpression}" +
               $"elseExpression is {ElseExpression}";
    }
}

public enum Operator
{
    // Arithmetic operators
    Add,
    Subtract,
    Multiply,
    Divide,

    // Comparison operators
    Equal,
    NotEqual,
    LessThan,
    GreaterThan,
    LessThanOrEqual,
    GreaterThanOrEqual,

    // Logical operators
    And,
    Or,

    // Unary operator (negation)
    Not
}


public static class OperatorExtensions
{
    public static Operator FromString(this string op)
    {
        switch (op)
        {
            case "+":
                return Operator.Add;
            case "-":
                return Operator.Subtract;
            case "*":
                return Operator.Multiply;
            case "/":
                return Operator.Divide;
            case "==":
                return Operator.Equal;
            case "!=":
                return Operator.NotEqual;
            case "<":
                return Operator.LessThan;
            case "<=":
                return Operator.LessThanOrEqual;
            case ">":
                return Operator.GreaterThan;
            case ">=":
                return Operator.GreaterThanOrEqual;
            case "&&":
                return Operator.And;
            case "||":
                return Operator.Or;
            case "!":
                return Operator.Not;
            default:
                throw new ArgumentException($"Invalid operator: {op}");
        }
    }
}