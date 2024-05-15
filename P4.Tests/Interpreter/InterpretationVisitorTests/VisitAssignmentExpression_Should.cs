using P4.Interpreter;
using P4.Interpreter.AST.Nodes;

namespace P4.Tests.Interpreter.InterpretationVisitorTests;

public class VisitAssignmentExpressionShould
{
    private readonly InterpretationVisitor _visitor = new();

    [Fact]
    public void VisitAssignment()
    {
        //Arrange
        var declaration = new VariableDeclaration(new IdentifierExpression("testVar"), new Type("Num"), new ConstantExpression(1));
        _visitor.Visit(declaration);
        
        var identifier = new IdentifierExpression("testVar");
        var expression = new ConstantExpression(1);
        var assignment = new AssignmentStatement(identifier, expression);

        //Act
        var result = _visitor.Visit(assignment);

        //Assert
        Assert.Null(result);
    }
    
}