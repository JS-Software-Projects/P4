using System.Collections.Generic;
using System.Linq;

namespace P4.Interpreter.AST;

public abstract class ASTNode {
    // Base class for all AST nodes
    public List<ASTNode> Children { get; set; } = new();
}

public class BlockNode : ASTNode {
    public List<ASTNode> Statements { get; } = new();
    
    public override string ToString() {
        return "{" + string.Join(" ", Statements.Where(s => s != null).Select(s => s.ToString())) + "}";
    }

}

public class FunctionDeclarationNode : ASTNode {
    public string type { get; set; }
    public string Name { get; set; }
    public List<ParameterNode> Parameters { get; set; }
    public ASTNode Body { get; set; }
}

public class VariableDeclarationNode : ASTNode {
    public string Type { get; set; }
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
    
    public override string ToString() {
        var leftExpr = Left != null ? Left.ToString() : "null";
        var rightExpr = Right != null ? Right.ToString() : "null";
        return $"({leftExpr} {Operator} {rightExpr})";
    }

}
public class TernaryExpressionNode : ASTNode {
    public ASTNode Condition { get; set; }
    public ASTNode TrueExpr { get; set; }
    public ASTNode FalseExpr { get; set; }

    public TernaryExpressionNode(ASTNode condition, ASTNode trueExpr, ASTNode falseExpr) {
        Condition = condition;
        TrueExpr = trueExpr;
        FalseExpr = falseExpr;
    }

    public override string ToString() {
        return $"({Condition} ? {TrueExpr} : {FalseExpr})";
    }
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

public class IdentifierNode : ASTNode {
    public string Identifier { get; set; }

    public IdentifierNode(string identifier) {
        Identifier = identifier;
    }

    public override string ToString()
    {
        return Identifier;
    }
}



