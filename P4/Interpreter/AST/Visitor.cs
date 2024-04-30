public interface Visitor
{
    //Statements
    void VisitAssignment(Assignment node);
    void VisitVariableDeclaration(VariableDeclaration node);
    void VisitPrint(Print node);
    void VisitIfBlock(IfBlock node);
    void VisitWhileBlock(WhileBlock node);
    void VisitBlock(Block node);
    void VisitFunctionCall(FunctionCall node);
    void VisitReturnStatement(ReturnStatement node);
    void VisitForLoop(ForLoop node);
   
    
    // Expressions
    void VisitConstantExpression(ConstantExpression node);
    void VisitIdentifierExpression(IdentifierExpression node);
    void VisitBinaryExpression(BinaryExpression node);
    void VisitUnaryExpression(UnaryExpression node);
    void VisitTernaryExpression(TernaryExpression node);
}