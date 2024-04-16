using System.Collections.Generic;

namespace P4.Interpreter.AST;

public abstract class ASTNode {
    // Base class for all AST nodes
    public List<ASTNode> Children { get; set; } = new List<ASTNode>();
}

public class FunctionDeclarationNode : ASTNode {
    public string Name { get; set; }
    public List<ParameterNode> Parameters { get; set; }
    public ASTNode Body { get; set; }
}

public class VariableDeclarationNode : ASTNode {
    public string VariableName { get; set; }
    public ASTNode Expression { get; set; }  // Can be null if no initial value is provided

    public override string ToString() {
        return Expression != null
            ? $"Variable Declaration: {VariableName} = {Expression}"
            : $"Variable Declaration: {VariableName}";
    }
}

public class ExpressionNode : ASTNode {
    public string Operator { get; set; }
    public ASTNode Left { get; set; }
    public ASTNode Right { get; set; }
}

public class ConstantNode : ASTNode {
    public object Value { get; set; }

    public ConstantNode(object value) {
        Value = value;
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}
