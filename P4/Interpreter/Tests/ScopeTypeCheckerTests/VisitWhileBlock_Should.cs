using System;
using P4.Interpreter.AST;
using Xunit;

namespace P4.Interpreter.Tests.ScopeTypeCheckerTests;

public class VisitWhileBlock_Should
{
    private readonly ScopeTypeChecker _checker = new();
    
    [Fact]
    public void Visit_WhileBlock_ConditionTypeIsBool_Success()
    {
        // Arrange
        var condition = new ConstantExpression(true);
        var block = new BlockStatement();  
        var node = new WhileBlock(condition, block);

        // Act
        var result = _checker.Visit(node);

        // Assert
        Assert.Equal(null, result);
    }
    
    [Fact]
    public void Visit_WhileBlock_ConditionTypeIsNotBool_ThrowsException()
    {
        // Arrange
        var condition = new ConstantExpression(1);
        var block = new BlockStatement();  
        var node = new WhileBlock(condition, block);

        // Act+Assert
        Assert.Throws<Exception>(() => _checker.Visit(node));
    }
}