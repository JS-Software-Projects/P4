using System;
using System.Collections.Generic;

namespace P4.Interpreter.AST;

public class TypeE : Type
{
    public List<Type> Args { get; set; }
    public TypeE(List<Type> args,string type) : base(type)
    {
        this.Args = args;
    }
    
    public override T Accept<T>(IASTVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
   
    public bool IsCorrectTypeE()
    {
        foreach (TypesE type in Enum.GetValues(typeof(TypesE)))
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

public enum TypesE
{
    Num,
    String,
    Bool,
    Void
}