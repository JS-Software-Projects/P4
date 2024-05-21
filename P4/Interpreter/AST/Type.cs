using System;using P4.Interpreter.AST;

public class Type : ASTNode {
    public string TypeName { get; set; }

    public Type(string type) {
        TypeName = type;
    }

    public override T Accept<T>(IASTVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }

    public bool IsCorrectType()
    {
        foreach (TypesEnum type in Enum.GetValues(typeof(TypesEnum)))
        {
            if (TypeName == type.ToString())
            {
                return true;
            }
        }

        return false;
    }

    public override bool Equals(object obj)
    {
        return obj is Type type &&
               TypeName == type.TypeName;
    }

    public override string ToString() {
        return TypeName;
    }
}

public enum TypesEnum
{
    Num,
    String,
    Bool
}
