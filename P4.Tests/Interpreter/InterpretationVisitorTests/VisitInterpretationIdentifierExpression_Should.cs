using P4.Interpreter.AST.Nodes;
using P4.Interpreter;

namespace P4.Tests.Interpreter.InterpretationVisitorTests;

public class VisitInterpretationIdentifierExpression_Should
{
    private readonly InterpretationVisitor _visitor = new();
    
    [Fact]
    public void Visit_IdentifierExpression_ReturnsValueFromEnvironment()
    {
        // Arrange
        
        var Expression = new ConstantExpression(1);
        var IdExpression = new IdentifierExpression("testVar");
        var Type = new Type("Num");
        var node = new VariableDeclaration(IdExpression, Type, Expression);
        _visitor.Visit(node);
    
        // Act
        var result = _visitor.Visit(IdExpression);

        // Assert
        Assert.Equal(1, result);
    }

   
}