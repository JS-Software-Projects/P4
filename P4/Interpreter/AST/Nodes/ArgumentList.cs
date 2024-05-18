using System.Collections.Generic;

namespace P4.Interpreter.AST.Nodes;

public class ArgumentList : ASTNode
{
    public List<Expression> Arguments { get; set; }

    public ArgumentList(List<Expression> arguments)
    {
        Arguments = arguments;
    }
    public ArgumentList(){}

    public override T Accept<T>(IASTVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }

    public override string ToString()
    {
        // Check if Arguments is null
        if (Arguments == null)
        {
            return "ArgumentList: (null)";
        }

        return $"ArgumentList: ({string.Join(", ", Arguments)})";
    }

}
