using P4.Interpreter;
using P4.Interpreter.AST.Nodes;

namespace P4.Tests.Interpreter.InterpretationVisitorTests;

public class VisitTernaryExpressionShould
{
    private readonly InterpretationVisitor _visitor = new();

    [Fact]
    public void VisitTernaryExpression_True()
    {
        //Arrange

        var condition = new ConstantExpression(true);
        var thenExpression = new ConstantExpression(1);
        var elseExpression = new ConstantExpression(2);

        //Act
        var node = new TernaryExpression(condition, thenExpression, elseExpression);

        var result = _visitor.Visit(node);

        //Assert
        Assert.Equal(1, result);
    }

    [Fact]
    public void VisitTernaryExpression_False()
    {
        //Arrange

        var condition = new ConstantExpression(false);
        var thenExpression = new ConstantExpression(1);
        var elseExpression = new ConstantExpression(2);

        //Act
        var node = new TernaryExpression(condition, thenExpression, elseExpression);

        var result = _visitor.Visit(node);

        //Assert
        Assert.Equal(2, result);
    }
}

