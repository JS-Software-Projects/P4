using System.Linq.Expressions;
using Antlr4.Runtime.Atn;
using P4.Interpreter;
using P4.Interpreter.AST.Nodes;
using BinaryExpression = P4.Interpreter.AST.Nodes.BinaryExpression;
using ConstantExpression = P4.Interpreter.AST.Nodes.ConstantExpression;

namespace P4.Tests.Interpreter.InterpretationVisitorTests;

public class VisitBinaryExpressionShould
{
    private readonly InterpretationVisitor _visitor = new();

    [Fact]
    public void VisitBinaryExpressionWithNumAndAdd_Succeeds()
    {
        var leftExpression = new ConstantExpression(2.0);
        var rightExpression = new ConstantExpression(2.0);

        var node = new BinaryExpression(leftExpression, Operator.Add, rightExpression);
        var result = _visitor.Visit(node);
        Assert.Equal( 4.0, result);
    }
    [Fact]
    public void VisitBinaryExpressionWithStringAndAdd_Succeeds()
    {
        var leftExpression = new ConstantExpression("Test");
        var rightExpression = new ConstantExpression("Success");

        var node = new BinaryExpression(leftExpression, Operator.Add, rightExpression);
        var result = _visitor.Visit(node);
        Assert.Equal( "TestSuccess", result);
    }
    [Fact]
    public void VisitBinaryExpressionWithNumAndSubtract_Succeeds()
    {
        var leftExpression = new ConstantExpression(5.0);
        var rightExpression = new ConstantExpression(4.0);

        var node = new BinaryExpression(leftExpression, Operator.Subtract, rightExpression);
        var result = _visitor.Visit(node);
        Assert.Equal( 1.0, result);
    }
    [Fact]
    public void VisitBinaryExpressionWithNumAndMultiply_Succeeds()
    {
        var leftExpression = new ConstantExpression(5.0);
        var rightExpression = new ConstantExpression(4.0);

        var node = new BinaryExpression(leftExpression, Operator.Multiply, rightExpression);
        var result = _visitor.Visit(node);
        Assert.Equal( 20.0, result);
    }
    [Fact]
    public void VisitBinaryExpressionWithNumAndDivide_Succeeds()
    {
        var leftExpression = new ConstantExpression(20.0);
        var rightExpression = new ConstantExpression(4.0);

        var node = new BinaryExpression(leftExpression, Operator.Divide, rightExpression);
        var result = _visitor.Visit(node);
        Assert.Equal( 5.0, result);
    }
    [Fact]
    public void VisitBinaryExpressionWithNumAndDivideWithZero_Throws()
    {
        var leftExpression = new ConstantExpression(20.0);
        var rightExpression = new ConstantExpression(0.0);

        var node = new BinaryExpression(leftExpression, Operator.Divide, rightExpression);
        
        var exception = Assert.Throws<Exception>(() => _visitor.Visit(node));
        Assert.Equal("Division by zero", exception.Message);
    }
    [Theory]
    [InlineData(3.0, 4.0, true)]
    [InlineData(5.0, 4.0, false)]
    public void VisitBinaryExpressionWithNumAndLessThan_Succeeds(double leftValue, double rightValue, bool expected)
    {
        var leftExpression = new ConstantExpression(leftValue);
        var rightExpression = new ConstantExpression(rightValue);

        var node = new BinaryExpression(leftExpression, Operator.LessThan, rightExpression);
        var result = _visitor.Visit(node);
        
        Assert.Equal( expected, result);
    }
    [Theory]
    [InlineData(4.0, 4.0, true)]
    [InlineData(3.0, 4.0, true)]
    [InlineData(5.0, 4.0, false)]
    public void VisitBinaryExpressionWithNumAndLessThanOrEqual_Succeeds(double leftValue, double rightValue, bool expected)
    {
        var leftExpression = new ConstantExpression(leftValue);
        var rightExpression = new ConstantExpression(rightValue);

        var node = new BinaryExpression(leftExpression, Operator.LessThanOrEqual, rightExpression);
        var result = _visitor.Visit(node);
    
        Assert.Equal(expected, result);
    }
    [Theory]
    [InlineData(10.0, 4.0, true)]
    [InlineData(3.0, 4.0, false)]
    public void VisitBinaryExpressionWithNumAndGreaterThan_Succeeds(double leftValue, double rightValue, bool expected)
    {
        var leftExpression = new ConstantExpression(leftValue);
        var rightExpression = new ConstantExpression(rightValue);

        var node = new BinaryExpression(leftExpression, Operator.GreaterThan, rightExpression);
        var result = _visitor.Visit(node);
        
        Assert.Equal( expected, result);
    }
    [Theory]
    [InlineData(10.0, 4.0, true)]
    [InlineData(10.0, 10.0, true)]
    [InlineData(3.0, 4.0, false)]
    public void VisitBinaryExpressionWithNumAndGreaterThanOrEqual_Succeeds(double leftValue, double rightValue, bool expected)
    {
        var leftExpression = new ConstantExpression(leftValue);
        var rightExpression = new ConstantExpression(rightValue);

        var node = new BinaryExpression(leftExpression, Operator.GreaterThanOrEqual, rightExpression);
        var result = _visitor.Visit(node);
        
        Assert.Equal( expected, result);
    }
    [Fact]
    public void VisitBinaryExpressionWithNumAndEqual_Succeeds()
    {
        var leftExpression = new ConstantExpression(1.0);
        var rightExpression = new ConstantExpression(1.0);

        var node = new BinaryExpression(leftExpression, Operator.Equal, rightExpression);
        var result = _visitor.Visit(node);
        
        Assert.Equal( true, result);
    }
    [Fact]
    public void VisitBinaryExpressionWithNumAndNotEqual_Succeeds()
    {
        var leftExpression = new ConstantExpression(1.0);
        var rightExpression = new ConstantExpression(1.0);

        var node = new BinaryExpression(leftExpression, Operator.NotEqual, rightExpression);
        var result = _visitor.Visit(node);
        
        Assert.Equal( false, result);
    }

    [Theory]
    [InlineData(true, true, true)]
    [InlineData(true, false, false)]
    public void VisitBinaryExpressionWithBoolAndAnd_Succeeds(bool leftValue, bool rightValue, bool expected)
    {
        var leftExpression = new ConstantExpression(leftValue);
        var rightExpression = new ConstantExpression(rightValue);

        var node = new BinaryExpression(leftExpression, Operator.And, rightExpression);
        var result = _visitor.Visit(node);

        Assert.Equal(expected, result);
    }
    [Theory]
    [InlineData(true, true, true)]
    [InlineData(true, false, true)]
    [InlineData(false, false, false)]
    public void VisitBinaryExpressionWithBoolAndOr_Succeeds(bool leftValue, bool rightValue, bool expected)
    {
        var leftExpression = new ConstantExpression(leftValue);
        var rightExpression = new ConstantExpression(rightValue);

        var node = new BinaryExpression(leftExpression, Operator.Or, rightExpression);
        var result = _visitor.Visit(node);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void VisitBinaryExpressionWithNumAndUnknownOperator_Throws()
    {
        var leftExpression = new ConstantExpression(20.0);
        var rightExpression = new ConstantExpression(0.0);

        var node = new BinaryExpression(leftExpression, Operator.Not, rightExpression);

        var exception = Assert.Throws<Exception>(() => _visitor.Visit(node));
        Assert.Equal("Unknown operator", exception.Message);
    }
}