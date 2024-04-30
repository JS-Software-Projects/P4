using System.Collections.Generic;

// Base class for all AST nodes
public abstract class ASTNode
{
    protected List<ASTNode> Children { get; set; } = new List<ASTNode>();
    
    public abstract void Accept(Visitor visitor);
}


// læste om denne, så vi måske kan tracke vores nodes, somehow? maybe
public class Location
{
    public int Line { get; set; }
    public int Column { get; set; }
}

