using System;
using P4.Interpreter;
using P4.Interpreter.AST.Nodes;

namespace P4.Tests.Interpreter.ScopeTypeCheckerTests;
using Xunit;
using P4.Interpreter.AST;

public class VisitTernaryExpression_Should
{
    private readonly ScopeTypeChecker _checker = new();
    
    [Fact]
    public void Visit_TernaryExpression_ReturnsType_Success()
    {
        var condition = new ConstantExpression(true);
        var trueExpression = new ConstantExpression(1);
        var falseExpression = new ConstantExpression(2);
        var node = new TernaryExpression(condition, trueExpression, falseExpression);
        var result = _checker.Visit(node);
        
        Assert.Equal("Num", result.TypeName);
    }
    
    [Fact]
    public void Visit_TernaryExpression_conditionNum_ThrowsException()
    {
        var condition = new ConstantExpression(9);
        var trueExpression = new ConstantExpression(1);
        var falseExpression = new ConstantExpression(2);
        var node = new TernaryExpression(condition, trueExpression, falseExpression);
        
        Assert.Throws<Exception>(() => _checker.Visit(node));
    }

    [Fact]
    public void Visit_TernaryExpression_conditionStringAndNum_ThrowsException()
    {
        var condition = new ConstantExpression(true);
        var trueExpression = new ConstantExpression("test");
        var falseExpression = new ConstantExpression(2);
        
        var node = new TernaryExpression(condition, trueExpression, falseExpression);
        
        Assert.Throws<Exception>(() => _checker.Visit(node));
    }
}