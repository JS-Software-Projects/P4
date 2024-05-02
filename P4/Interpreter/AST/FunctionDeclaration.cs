using P4.Interpreter.AST;
using System.Collections.Generic;

public class FunctionDeclaration : ASTNode
{
    public string FunctionName { get; set; }
    public List<ParameterNode> Parameters { get; set; }
    public List<Statement> Statements { get; set; }
    public string ReturnType { get; set; }

    public FunctionDeclaration(string functionName, List<ParameterNode> parameters, List<Statement> statements, string returnType)
    {
        FunctionName = functionName;
        Parameters = parameters;
        Statements = statements;
        ReturnType = returnType;
    }

    public override T Accept<T>(IASTVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }

    public override string ToString()
    {
        return $"FunctionDeclaration: {ReturnType} {FunctionName}({string.Join(", ", Parameters)})";
    }
}