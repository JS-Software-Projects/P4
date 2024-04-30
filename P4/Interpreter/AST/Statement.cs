using System.Collections.Generic;
using P4.Interpreter.AST;

public class Statement : ASTNode
{
    public override T Accept<T>(IASTVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}

public class Assignment : Statement
{
    public string VariableName { get; set; }
    public Expression Expression { get; set; }
    
    public override T Accept<T>(IASTVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}

public class VariableDeclaration : Statement
{
    public string VariableName { get; set; }
    public string Type { get; set; }
    public Expression Expression { get; set; }
    
    public override T Accept<T>(IASTVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }

    public override string ToString()
    {
        return $"VariableDeclaration: {Type} {VariableName} = {Expression}";
    }
}

public class Print : Statement
{
    public Expression Expression { get; set; }
    public override T Accept<T>(IASTVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}

public class IfBlock : Statement
{
    public Expression Condition { get; set; }
    public Block Block { get; set; }
    public Block ElseBlock { get; set; }
    public override T Accept<T>(IASTVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}

public class WhileBlock : Statement
{
    public Expression Condition { get; set; }
    public Block Block { get; set; }
    public override T Accept<T>(IASTVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}

public class Block : Statement
{
    public List<Statement> Statements { get; set; } = new List<Statement>();
    public override T Accept<T>(IASTVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}

public class FunctionCall : Statement
{
    public string FunctionName { get; set; }
    public List<Expression> Arguments { get; set; } = new List<Expression>();
    public override T Accept<T>(IASTVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}

public class ReturnStatement : Statement
{
    public Expression Expression { get; set; }
    public override T Accept<T>(IASTVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}

public class ForLoop : Statement
{
    public Statement Initialization { get; set; }
    public Expression Condition { get; set; }
    public Statement Increment { get; set; }
    public Block Block { get; set; }
    public override T Accept<T>(IASTVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}