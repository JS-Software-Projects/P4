﻿using System.Collections.Generic;
using P4.Interpreter.AST;

namespace P4.Interpreter.AST.Nodes;
public abstract class Statement : ASTNode { }

public class AssignmentStatement : Statement
{
    public IdentifierExpression VariableName { get; set; }
    public Expression Expression { get; set; }
    
    public override T Accept<T>(IASTVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
    public AssignmentStatement(IdentifierExpression variableName, Expression expression)
    {
        VariableName = variableName;
        Expression = expression;
    }

    public override string ToString()
    
    {
        return $"AssignmentStatement: {VariableName} = {Expression}";
    }
}

public class VariableDeclaration : Statement
{
    public IdentifierExpression VariableName { get; set; }
    public Type Type { get; set; }
    public Expression Expression { get; set; }

    public VariableDeclaration(IdentifierExpression variableName, Type type, Expression expression)
    {
        VariableName = variableName;
        Type = type;
        Expression = expression;
    }
    
    public VariableDeclaration(IdentifierExpression variableName, Type type)
    {
        VariableName = variableName;
        Type = type;
    }

    public override T Accept<T>(IASTVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }

    public override string ToString()
    {
        return $"VariableDeclaration: {Type} {VariableName} = {Expression}";
    }
}

public class PrintStatement : Statement
{
    public Expression Expression { get; set; }

    public override T Accept<T>(IASTVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
    public PrintStatement(Expression expression)
    {
        Expression = expression;

    }
    public override string ToString()
    {
        return $"PrintStatement: {Expression}";
    }
}

public class IfBlock : Statement
{
    public Expression Condition { get; set; }
    public BlockStatement Block { get; set; }
    public BlockStatement ElseBlock { get; set; }
    public override T Accept<T>(IASTVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }

    public IfBlock(Expression condition, BlockStatement block, BlockStatement elseBlock)
    {
        Condition = condition;
        Block = block;
        ElseBlock = elseBlock;
    }

    public override string ToString()
    {
        return $"IfBlockStatement: Condition is = {Condition}, " +
               $"Block is = {Block}, " +
               $"ElseBlock is = {ElseBlock}";
    }
}

public class WhileBlock : Statement
{
    public Expression Condition { get; set; }
    public BlockStatement Block { get; set; }
    public override T Accept<T>(IASTVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }

    public WhileBlock(Expression condition, BlockStatement block)
    {
        Condition = condition;
        Block = block;
    }

    public override string ToString()
    {
        return $"WhileBlockStatement: Condition is = {Condition}, " +
               $"Block is = {Block}";
    }
}

public class BlockStatement : Statement
{
    public List<Statement> Statements { get; set; } = new List<Statement>();
    public override T Accept<T>(IASTVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }

   public BlockStatement()
    {
        Statements = new List<Statement>();
    }

    public override string ToString()
    {
        return $"BlockStatements: {string.Join(", ", Statements)}";
    }
}

public class FunctionCallStatement : Statement
{
    public string FunctionName { get; set; }
    public ArgumentList Arguments { get; set; } 
    public override T Accept<T>(IASTVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }

    public FunctionCallStatement(string functionName, ArgumentList arguments)
    {
        FunctionName = functionName;
        Arguments = arguments;
    }

    public override string ToString()
    {
        return $"FunctionCall: {FunctionName}({string.Join(", ", Arguments)})";
    }
}
public class GameObjectDeclaration : Statement
{
    public ObjectType ObjectType { get; set; }
    public IdentifierExpression ObjectName { get; set; }
    public ArgumentList ArgumentLists { get; set; }
    private object GameObject { get; set; }

    public GameObjectDeclaration(ObjectType objectType, IdentifierExpression objectName, ArgumentList argument)
    {
        ObjectType = objectType;
        ObjectName = objectName;
        ArgumentLists = argument;
    }

    public override T Accept<T>(IASTVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
    
    public object GetGameObject()
    {
        return GameObject;
    }
    public void SetGameObject(object obj)
    {
        GameObject = obj;
    }

    public override string ToString()
    {
        return $"GameObjectDeclaration: {ObjectType} {ObjectName} = {ArgumentLists}";
    }
}


public class GameObjectMethodCall : Statement
{
    public IdentifierExpression ObjectName { get; set; }
    public string MethodName { get; set; }
    public ArgumentList ArgumentList { get; set; }
    
    public GameObjectMethodCall(IdentifierExpression objectName, string methodName, ArgumentList argumentList)
    {
        ObjectName = objectName;
        MethodName = methodName;
        ArgumentList = argumentList;
    }
    public override T Accept<T>(IASTVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
    public override string ToString()
    {
        return $"GameObjectMethodCall: {ObjectName}.{MethodName}({string.Join(", ", ArgumentList)})";
    }
}

public class ReturnStatement : Statement
{
    public Expression Expression { get; set; }
    public override T Accept<T>(IASTVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }

    public ReturnStatement(Expression expression)
    {
        Expression = expression;
    }

    public override string ToString()
    {
        return $"ReturnStatement: {Expression}";
    }
}

public class ForLoopStatement : Statement
{
    public Statement Initialization { get; set; }
    public Expression Condition { get; set; }
    public Statement Increment { get; set; }
    public BlockStatement Block { get; set; }
    public override T Accept<T>(IASTVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }

    public ForLoopStatement(Statement initialization, Expression condition, Statement increment, BlockStatement block)
    {
        Initialization = initialization;
        Condition = condition;
        Increment = increment;
        Block = block;
    }

    public override string ToString()
    {
        return $"ForLoop: Initialization is = {Initialization}, " +
               $"Condition is = {Condition}, " +
               $"Increment is = {Increment}, " +
               $"Block is = {Block}";
    }
}