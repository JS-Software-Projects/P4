namespace P4.Interpreter.AST;

public interface IASTVisitor<T>
{
    T Visit(ASTNode node);
    T Visit(BlockStatement node);
    T Visit(BinaryExpression node);
    T Visit(UnaryExpression node);
    T Visit(TernaryExpression node);
    T Visit(AssignmentStatement node);
    T Visit(FunctionCallStatement node);
    T Visit(VariableDeclaration node);
    T Visit(Expression node);
    T Visit(Statement node);
    T Visit(ConstantExpression node);
    T Visit(IdentifierExpression node);
    //T Visit(BinOpNode node);
    //T Visit(UnaryOpNode node);
    T Visit(ParameterNode node);
    T Visit(FunctionDeclaration node);

    T Visit(PrintStatement node);
    T Visit(IfBlock node);
    T Visit(WhileBlock node);
    T Visit(ReturnStatement node);
    T Visit(ForLoopStatement node);
  //  T Visit(IfNode node);
   // T Visit(WhileNode node);
  //  T Visit(ReturnNode node);
   // T Visit(ForNode node);

}
