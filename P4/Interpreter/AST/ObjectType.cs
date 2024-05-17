using System;

namespace P4.Interpreter.AST;

public class ObjectType : ASTNode
{
    public string TypeName { get; set; }
    
    public ObjectType(string objectType)
    {
        TypeName = objectType;
    }
    public override T Accept<T>(IASTVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
    public bool IsCorrectType()
    {
        foreach (ObjectTypesEnum type in Enum.GetValues(typeof(ObjectTypesEnum)))
        {
            if (TypeName == type.ToString())
            {
                return true;
            }
        }

        return false;
    }
}
public enum ObjectTypesEnum
{
    Tower,
    Hero,
}