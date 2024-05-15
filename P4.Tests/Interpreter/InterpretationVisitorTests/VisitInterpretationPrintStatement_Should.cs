using P4.Interpreter;
using P4.Interpreter.AST.Nodes;

namespace P4.Tests.Interpreter.InterpretationVisitorTests;

public class VisitInterpretationPrintStatement_Should
{
    private readonly InterpretationVisitor _visitor = new();
    
    [Fact]
    public void PrintStatement_Constructor_SetsPropertiesCorrectly()
    {
        // Arrange
        var expression = new ConstantExpression(5);
        
        // Act
        var printStatement = new PrintStatement(expression);
        _visitor.Visit(printStatement);
        
        // Assert
        Assert.Equal(expression, printStatement.Expression);
    }
}