using System;
using P4.Interpreter;
using P4.Interpreter.AST.Nodes;

namespace P4.Tests.Interpreter.ScopeTypeCheckerTests;
using Xunit;
using P4.Interpreter.AST;

public class VisitForLoopStatement_Should
{
    private readonly ScopeTypeChecker _checker = new();
    
    [Fact]
    public void Visit_ForLoopStatement_ReturnsType_Success()
    {
        var initialization = new VariableDeclaration( new IdentifierExpression("testVar"), new Type("Num"), new ConstantExpression(1));
        var condition = new ConstantExpression(true);
        var increment = new AssignmentStatement( new IdentifierExpression("testVar"), new ConstantExpression(1));
        var block = new BlockStatement();

        var node = new ForLoopStatement(initialization, condition, increment, block);
        var result = _checker.Visit(node);
        
        Assert.Equal(null, result);
    }
    
    [Fact]
    public void Visit_ForLoopStatement_conditionNum_ThrowsException()
    {
        
        var initialization = new VariableDeclaration(new IdentifierExpression("testVar"), new Type("Num"), new ConstantExpression(1));
        var condition = new ConstantExpression(9);
        var increment = new AssignmentStatement( new IdentifierExpression("testVar"), new ConstantExpression(1));
        var block = new BlockStatement();

        var node = new ForLoopStatement(initialization, condition, increment, block);
        
        Assert.Throws<Exception>(() => _checker.Visit(node));
    }
}