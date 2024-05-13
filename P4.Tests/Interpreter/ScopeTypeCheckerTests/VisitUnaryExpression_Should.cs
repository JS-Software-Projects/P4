using System;
using P4.Interpreter;
using P4.Interpreter.AST.Nodes;
namespace P4.Tests.Interpreter.ScopeTypeCheckerTests;
using P4.Interpreter.AST;
using Xunit;

public class VisitUnaryExpressionShould
{
    private readonly ScopeTypeChecker _checker = new();
    
    [Fact]
    public void Visit_UnaryExpression_ReturnBool_NOT_Success()
    {
        // Arrange
        var VarExpression = new ConstantExpression(true);
        var UnaryOperator = Operator.Not;
        var node = new UnaryExpression(UnaryOperator, VarExpression);
        
        // Act
        var result = _checker.Visit(node);
        
        // Assert
        Assert.Equal("Bool", result.TypeName);
    }
    
    [Fact]
    public void Visit_unaryExpression_NotOperator_ThrowsException()
    {
        // Arrange
        var VarExpression = new ConstantExpression(1);
        var UnaryOperator = Operator.Not;
        var node = new UnaryExpression(UnaryOperator, VarExpression);
        
        // Act + Assert
        Assert.Throws<Exception>(() => _checker.Visit(node));
    }
    
    
    [Fact]
    public void Visit_UnaryExpression_ReturnNum_Subtract_Success()
    {
        // Arrange
        var VarExpression = new ConstantExpression(1);
        var UnaryOperator = Operator.Subtract;
        var node = new UnaryExpression(UnaryOperator, VarExpression);
        
        // Act
        var result = _checker.Visit(node);
        
        // Assert
        Assert.Equal("Num", result.TypeName);
    }
    
    [Fact]
    public void Visit_UnaryExpression_SubtractOperator_ThrowsException()
    {
        // Arrange
        var VarExpression = new ConstantExpression(true);
        var UnaryOperator = Operator.Subtract;
        var node = new UnaryExpression(UnaryOperator, VarExpression);
        
        // Act + Assert
        Assert.Throws<Exception>(() => _checker.Visit(node));
    }
    
    [Fact]
    public void Visit_UnaryExpression_AddOperator_ThrowsException()
    {
        // Arrange
        var VarExpression = new ConstantExpression(8);
        var UnaryOperator = Operator.Add;
        var node = new UnaryExpression(UnaryOperator, VarExpression);
        
        // Act + Assert
        Assert.Throws<Exception>(() => _checker.Visit(node));
    }
    
        
        
        
}