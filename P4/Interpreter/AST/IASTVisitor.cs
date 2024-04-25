namespace P4.Interpreter.AST;

public interface IASTVisitor<T>
{
    T Visit(ASTNode node);
    T Visit(BlockNode node);
    T Visit(FunctionDeclarationNode node);
    T Visit(VariableDeclarationNode node);
    T Visit(TypeNode node);
    T Visit(ExpressionNode node); // binop og unop
    T Visit(ConstantNode node);
    T Visit(IdentifierNode node);
    //T Visit(BinOpNode node);
    //T Visit(UnaryOpNode node);
    T Visit(FunctionCallNode node);
    T Visit(ParameterNode node);
    T Visit(IfNode node);
    T Visit(WhileNode node);
    T Visit(ReturnNode node);
    T Visit(ForNode node);
    

    
}
