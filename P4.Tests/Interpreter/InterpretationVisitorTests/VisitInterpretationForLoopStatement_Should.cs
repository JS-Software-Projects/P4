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
        var declaration = new VariableDeclaration(new IdentifierExpression("i"), new Type("Num"), new ConstantExpression(0.0));
        _visitor.Visit(declaration);
        var initialization = new AssignmentStatement(new IdentifierExpression("i"), new ConstantExpression(0.0));
        var condition = new BinaryExpression(new IdentifierExpression("i"), Operator.LessThan, new ConstantExpression(10.0));
        var increment = new AssignmentStatement(new IdentifierExpression("i"), new BinaryExpression(new IdentifierExpression("i"),Operator.Add, new ConstantExpression(1.0)));
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