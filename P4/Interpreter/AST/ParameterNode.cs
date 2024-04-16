﻿namespace P4.Interpreter.AST;

public class ParameterNode : ASTNode {
    private string ParameterName { get; set; }
    private string Type { get; set; }  // Include this if your language supports or requires type annotations

    public ParameterNode(string parameterName, string type) {
        this.ParameterName = parameterName;
        this.Type = type;
    }

    // Optionally, you can override ToString() for easier debugging and testing
    public override string ToString() {
        return $"{Type} {ParameterName}";
    }
}