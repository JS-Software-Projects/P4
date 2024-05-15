using P4.Interpreter;
using P4.Interpreter.AST.Nodes;

namespace P4.Tests.Interpreter.InterpretationVisitorTests;

public class VisitUnaryExpression_Should
{
    private readonly InterpretationVisitor _visitor = new();
    [Fact]
    public void VisitUnaryExpressionWithBoolAndNot_Succeeds()
    {
        var operand = new ConstantExpression(true);
        

        var node = new UnaryExpression( Operator.Not, operand);
        var result = _visitor.Visit(node);
        
        Assert.Equal( false, result);
    }
    [Fact]
    public void VisitUnaryExpressionWithBoolAndSubtract_Succeeds()
    {
        var operand = new ConstantExpression(4.0);
        

        var node = new UnaryExpression( Operator.Subtract, operand);
        var result = _visitor.Visit(node);
        
        Assert.Equal( -4.0, result);
    }
    [Fact]
    public void VisitUnaryExpressionWithNumAndUnknownOperator_Throws()
    {
        var operand = new ConstantExpression(20.0);
        

        var node = new UnaryExpression( Operator.Add, operand);

        var exception = Assert.Throws<Exception>(() => _visitor.Visit(node));
        Assert.Equal("Unknown operator", exception.Message);
    }
}