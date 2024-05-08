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
        foreach (Types type in Enum.GetValues(typeof(Types)))
        {
            if (TypeName == type.ToString())
            {
                return true;
            }
        }

        return false;
    }
   

    public override string ToString() {
        return TypeName;
    }
}

public enum Types
{
    Num,
    String,
    Bool
}
