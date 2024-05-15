using System;

namespace P4.Interpreter.AST;

public class TypeClass : ASTNode
{
    public string ClassName { get; set; }
    
    public TypeClass(string className)
    {
        ClassName = className;
    }
    public override T Accept<T>(IASTVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
    public bool isCorrectType()
    {
        foreach (classTypes type in Enum.GetValues(typeof(classTypes)))
        {
            if (ClassName == type.ToString())
            {
                return true;
            }
        }

        return false;
    }
}
public enum classTypes
{
    Tower,
    Hero,
}