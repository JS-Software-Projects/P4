﻿using P4.Interpreter.AST;
using System.Collections.Generic;

public class FunctionDeclaration : ASTNode
{
    public IdentifierExpression FunctionName { get; set; }
    public ParameterList ParameterList { get; set; }
    public BlockStatement Statements { get; set; }
    public Type ReturnType { get; set; }

    public FunctionDeclaration(Type returnType,IdentifierExpression functionName, ParameterList parameters, BlockStatement statements)
    {
        FunctionName = functionName;
        ParameterList = parameters;
        Statements = statements;
        ReturnType = returnType;
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