namespace P4.Interpreter.AST;

public class ParameterNode : ASTNode {
    public string ParameterName { get; set; }
    public string Type { get; set; }  // Include this if your language supports or requires type annotations

    public ParameterNode(string parameterName, string type) {
        this.ParameterName = parameterName;
        this.Type = type;
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
