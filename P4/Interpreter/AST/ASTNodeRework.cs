using System.Collections.Generic;
using P4.Interpreter.AST;

// Base class for all AST nodes
public abstract class ASTNode
{
    protected List<ASTNode> Children { get; set; } = new List<ASTNode>();
    
    public abstract T Accept<T>(IASTVisitor<T> visitor);
}


// læste om denne, så vi måske kan tracke vores nodes, somehow? maybe
public class Location
{
    public int Line { get; set; }
    public int Column { get; set; }
}

