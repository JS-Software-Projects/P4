using System.Collections.Generic;

namespace P4.Interpreter.AST.Nodes;

public class ParameterList : ASTNode
{
    public List<ParameterNode> Parameters { get; set; }

    public ParameterList(List<ParameterNode> parameters)
    {
        Parameters = parameters;
    }

    public override T Accept<T>(IASTVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }

    public override string ToString()
    {
        return $"ParameterList: ({string.Join(", ", Parameters)})";
    }
}