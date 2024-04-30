public abstract class Expression : ASTNode // Abstract base class
{
    // No properties in the base class, subclasses will define their own value types
    
    public override void Accept(Visitor visitor)
    {
        // No specific logic needed for expressions because we don't have any children nodes.
    }
}

public class IdentifierExpression : Expression
{
    public string Name { get; set; }
}

public class ConstantExpression : Expression
{
    public object Value { get; set; }
}

public class BinaryExpression : Expression
{
    public Expression Left { get; set; }
    public Operator Operator { get; set; } // Use an enum for operators
    public Expression Right { get; set; }
}

public class UnaryExpression : Expression
{
    public Operator Operator { get; set; } // Use an enum for operators
    public Expression Operand { get; set; }
}

public class TernaryExpression : Expression
{
    public Expression Condition { get; set; }
    public Expression ThenExpression { get; set; }
    public Expression ElseExpression { get; set; }
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
