using System;
using System.Collections.Generic;

namespace P4.Interpreter.AST;

public class TypeExtended : Type
{
    public List<Type> Args { get; set; }
    public TypeExtended(List<Type> args,string type) : base(type)
    {
        this.Args = args;
    }
    
    public override T Accept<T>(IASTVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
   
    public bool IsCorrectTypeE()
    {
        foreach (TypesExtendedEnum type in Enum.GetValues(typeof(TypesExtendedEnum)))
        {
            if (TypeName == type.ToString())
            {
                return true;
            }
        }
        return false;
    }

    public override string ToString()
    {
        var argList = "";
        foreach (var args in Args)
        {
            argList+= args.TypeName+",";
        }
        return argList + "-> "+ TypeName;
    }
}

public enum TypesExtendedEnum
{
    Num,
    String,
    Bool,
    Void
}