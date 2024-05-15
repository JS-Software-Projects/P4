using System;
using P4.Interpreter;
using P4.Interpreter.AST;
using P4.Interpreter.AST.Nodes;
using Xunit;


namespace P4.Tests.Interpreter.ScopeTypeCheckerTests;

public class VisitIfBlock_Should
{
    private readonly ScopeTypeChecker _checker = new();

    
    [Fact]
    public void Visit_IfBlock_ConditionTypeIsBool_Success()
    {
        // Arrange
        var condition = new ConstantExpression(true);
        var block = new BlockStatement();  
        var elseBlock = new BlockStatement();
        var node = new IfBlock(condition, block, elseBlock);

        // Act
        var result = _checker.Visit(node);

        // Assert
        Assert.Equal(null, result);
    }
    
    [Fact]
    public void Visit_IfBlock_ConditionTypeIsNotBool_ThrowsException()
    {
        // Arrange
        var condition = new ConstantExpression(1);
        var block = new BlockStatement();  
        var elseBlock = new BlockStatement();
        var node = new IfBlock(condition, block, elseBlock);

        // Act+Assert
        Assert.Throws<Exception>(() => _checker.Visit(node));
    }
}