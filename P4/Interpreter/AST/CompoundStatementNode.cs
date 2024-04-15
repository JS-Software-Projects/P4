using System.Collections.Generic;
using System.Text;

namespace P4.Interpreter.AST;

public class CompoundStatementNode : ASTNode {
    public CompoundStatementNode() {
        this.Children = new List<ASTNode>();
    }

    public override string ToString() {
        var builder = new StringBuilder("Compound Statement:\n");
        foreach (var child in Children) {
            builder.AppendLine(child.ToString());
        }
        return builder.ToString();
    }
}
