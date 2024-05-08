namespace P4.Interpreter.AST;

public class ParameterNode : ASTNode {
    public IdentifierExpression ParameterName { get; set; }
    public Type Type { get; set; }  // Include this if your language supports or requires type annotations

    public ParameterNode(IdentifierExpression parameterName, Type type) {
        ParameterName = parameterName;
        Type = type;
    }

    // Optionally, you can override ToString() for easier debugging and testing
    public override T Accept<T>(IASTVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }

    public override string ToString() {
        return $"{Type} {ParameterName}";
    }
}
