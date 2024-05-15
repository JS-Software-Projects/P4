using P4.Interpreter;
using P4.Interpreter.AST.Nodes;

namespace P4.Tests.Interpreter.InterpretationVisitorTests;

public class InterpretationVisitorTests
{
    private readonly InterpretationVisitor _visitor = new();

    [Fact]
    public void VisitBlockStatement_ReturnsNull_WhenNoReturnStatement()
    {
        // Arrange
        var node = new BlockStatement { Statements = new List<Statement> { new PrintStatement(new ConstantExpression("Hello")) } };

        // Act
        var result = _visitor.Visit(node);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void VisitBlockStatement_ReturnsValue_WhenContainsReturnStatement()
    {
        // Arrange
        var returnStatement = new ReturnStatement(new ConstantExpression(1));
        var node = new BlockStatement { Statements = new List<Statement> { returnStatement } };

        // Act
        var result = _visitor.Visit(node);

        // Assert
        Assert.Equal(1, result);
    }
}