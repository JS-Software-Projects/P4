using System;
using P4.Interpreter;
using P4.Interpreter.AST;
using Xunit;
using P4.Interpreter.AST.Nodes;

namespace P4.Tests.Interpreter.ScopeTypeCheckerTests;
public class VisitConstantExpressions_Should
{
    private readonly ScopeTypeChecker _checker = new();
    [Fact]
    public void Visit_ConstantExpression_ReturnsNumType()
    {
        // Arrange
        var node = new ConstantExpression(1);

        // Act
        var result = _checker.Visit(node);

        // Assert
        Assert.Equal("Num", result.TypeName);
    }
    
    [Fact]
    public void Visit_ConstantExpression_ReturnsBoolType()
    {
        // Arrange
        var node = new ConstantExpression(true);

        // Act
        var result = _checker.Visit(node);

        // Assert
        Assert.Equal("Bool", result.TypeName);
    }
    
    [Fact]
    public void Visit_ConstantExpression_ReturnsStringType()
    {
        // Arrange
        var node = new ConstantExpression("Hej");

        // Act
        var result = _checker.Visit(node);

        // Assert
        Assert.Equal("String", result.TypeName);
    }
    
    [Fact]
    public void Visit_ConstantExpression_ReturnsNullType()
    {
        // Arrange
        var node = new ConstantExpression(null);

        // Act+Assert
        Assert.Throws<Exception>(() => _checker.Visit(node));
    }
    
    
    
}