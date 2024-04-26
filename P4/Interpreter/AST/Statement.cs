using System.Collections.Generic;

public class Statement : ASTNode
{
    public override void Accept(Visitor visitor)
    {
        // No specific logic for Statement, 
        // but call Accept on children if applicable
        foreach (var child in Children)
        {
            child.Accept(visitor);
        }
    }
}

public class Assignment : Statement
{
    public string VariableName { get; set; }
    public Expression Expression { get; set; }
    
    public override void Accept(Visitor visitor)
    {
        visitor.VisitAssignment(this);
    }
}

public class VariableDeclaration : Statement
{
    public string VariableName { get; set; }
    public string Type { get; set; }
    public Expression Expression { get; set; }
    
    public override void Accept(Visitor visitor)
    {
        visitor.VisitVariableDeclaration(this);
    }
}

public class Print : Statement
{
    public Expression Expression { get; set; }
}

public class IfBlock : Statement
{
    public Expression Condition { get; set; }
    public Block Block { get; set; }
    public Block ElseBlock { get; set; }
}

public class WhileBlock : Statement
{
    public Expression Condition { get; set; }
    public Block Block { get; set; }
}

public class Block
{
    public List<Statement> Statements { get; set; } = new List<Statement>();
}

public class FunctionCall : Statement
{
    public string FunctionName { get; set; }
    public List<Expression> Arguments { get; set; } = new List<Expression>();
}

public class ReturnStatement : Statement
{
    public Expression Expression { get; set; }
}

public class ForLoop : Statement
{
    public Statement Initialization { get; set; }
    public Expression Condition { get; set; }
    public Statement Increment { get; set; }
    public Block Block { get; set; }
}