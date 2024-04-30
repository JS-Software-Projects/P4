using P4.Interpreter.AST;

namespace P4.Interpreter.AST;
public abstract class Expression : ASTNode // Abstract base class
{
    // No properties in the base class, subclasses will define their own value types
    public override T Accept<T>(IASTVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}

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
}

public class ConstantExpression : Expression
{
    public object Value { get; set; }
    public override T Accept<T>(IASTVisitor<T> visitor)
    {
        return visitor.Visit(this);
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
