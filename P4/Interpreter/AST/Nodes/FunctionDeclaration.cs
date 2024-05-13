using P4.Interpreter.AST;
using System.Collections.Generic;
using P4.Interpreter;

namespace P4.Interpreter.AST.Nodes;
public class FunctionDeclaration : ASTNode
{
    public IdentifierExpression FunctionName { get; set; }
    public ParameterList ParameterList { get; set; }
    public BlockStatement Block { get; set; }
    public Type ReturnType { get; set; }

    private Environment environment { get; set; }

    public FunctionDeclaration(Type returnType,IdentifierExpression functionName, ParameterList parameters, BlockStatement statements)
    {
        FunctionName = functionName;
        ParameterList = parameters;
        Block = statements;
        ReturnType = returnType;
    }

    public void SetEnvironment(Environment _environment)
    {
        environment = _environment;
    }

    public Environment GetEnvironment()
    {
        return environment;
    }
    
    public override T Accept<T>(IASTVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }

    public override string ToString()
    {
        return $"FunctionDeclaration: {ReturnType} {FunctionName}({string.Join(", ", ParameterList)})";
    }
}
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