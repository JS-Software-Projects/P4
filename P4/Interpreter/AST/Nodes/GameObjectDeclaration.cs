using System;
using System.Collections.Generic;

namespace P4.Interpreter.AST.Nodes;

public class GameObjectDeclaration : ASTNode
{
    public TypeClass ClassType { get; set; }
    public IdentifierExpression ObjectName { get; set; }
    public ArgumentList ArgumentLists { get; set; }
    private object GameObject { get; set; }

    public GameObjectDeclaration(TypeClass classType, IdentifierExpression objectName, ArgumentList argument)
    {
        ClassType = classType;
        ObjectName = objectName;
        ArgumentLists = argument;
    }

    public override T Accept<T>(IASTVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
    
    public Object GetGameObject()
    {
        return GameObject;
    }
    public void SetGameObject(Object obj)
    {
        GameObject = obj;
    }
    
}

