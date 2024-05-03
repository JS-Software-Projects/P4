using P4.Interpreter.AST;

public class Type : ASTNode {
    public string TypeName { get; set; }

    public Type(string type) {
        TypeName = type;
    }

    public override T Accept<T>(IASTVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }

    public override string ToString() {
        return TypeName;
    }
}