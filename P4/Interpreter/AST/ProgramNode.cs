using System.Collections.Generic;
using System.Text;

namespace P4.Interpreter.AST;

public class ProgramNode : ASTNode {
    // This constructor could initialize any collections or default values
    public ProgramNode() {
        this.Children = new List<ASTNode>();
    }

    // Optionally, you might include methods to add nodes directly
    public void AddChild(ASTNode child) {
        Children.Add(child);
    }
    public List<ASTNode> GetChildren() {
        return Children;
    }

    // Override ToString() to help in debugging and visualizing the tree
    public override string ToString() {
        var builder = new StringBuilder();
        builder.AppendLine("AST:");
        foreach (var child in Children) {
            builder.AppendLine(child.ToString());
        }
        return builder.ToString();
    }
}
