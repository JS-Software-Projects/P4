using P4.Interpreter;
using P4.Interpreter.AST.Nodes;

namespace P4.Tests.Interpreter.InterpretationVisitorTests;

public class VisitInterpretationForLoopStatement_Should
{
    private readonly InterpretationVisitor _visitor = new();
    
    [Fact]
    public void ForLoopStatement_Constructor_SetsPropertiesCorrectly()
    {
        // Arrange
        var initialization = new AssignmentStatement(new IdentifierExpression("i"), new ConstantExpression(0));
        var condition = new BinaryExpression(new IdentifierExpression("i"), new ConstantExpression(10), BinaryOperator.LessThan);
        var increment = new AssignmentStatement(new IdentifierExpression("i"), new BinaryExpression(new IdentifierExpression("i"), new ConstantExpression(1), BinaryOperator.Add));
        var block = new BlockStatement();

        // Act
        var forLoopStatement = new ForLoopStatement(initialization, condition, increment, block);
        _visitor.Visit(forLoopStatement);
        
        // Assert
        Assert.Equal(initialization, forLoopStatement.Initialization);
        Assert.Equal(condition, forLoopStatement.Condition);
        Assert.Equal(increment, forLoopStatement.Increment);
        Assert.Equal(block, forLoopStatement.Block);
    }
}