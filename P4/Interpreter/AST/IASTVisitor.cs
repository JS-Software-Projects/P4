namespace P4.Interpreter.AST;
using P4.Interpreter.AST.Nodes;

public interface IASTVisitor<T>
{
    T Visit(ASTNode node);
    T Visit(ProgramNode node);
    T Visit(BlockStatement node);
    T Visit(BinaryExpression node);
    T Visit(UnaryExpression node);
    T Visit(TernaryExpression node);
    T Visit(AssignmentStatement node);
    T Visit(FunctionCallStatement node);
    T Visit(GameObjectDeclaration node);
    T Visit(GameObjectMethodCall node);
    T Visit(VariableDeclaration node);
    T Visit(ConstantExpression node);
    T Visit(IdentifierExpression node);
    T Visit(ParameterNode node);
    T Visit(FunctionDeclaration node);
    T Visit(PrintStatement node);
    T Visit(IfBlock node);
    T Visit(WhileBlock node);
    T Visit(ReturnStatement node);
    T Visit(ForLoopStatement node);
}
