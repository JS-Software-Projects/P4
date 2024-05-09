using System;
using P4.Interpreter.AST;
using Xunit;

namespace P4.Interpreter.Tests.ScopeTypeCheckerTests;


public class VisitIdentifierExpression_Should
{
    private readonly ScopeTypeChecker _checker = new();
    [Fact]
    public void Visit_IdentifierExpression_ReturnsType_Success()
    {
        // Arrange
        var VarExpression = new ConstantExpression(1);
        var VarIdExpression = new IdentifierExpression("testVar");
        var VarType = new Type("Num");
        var node = new VariableDeclaration(VarIdExpression, VarType, VarExpression);
        _checker.Visit(node);
        
        // Act
        var result2 = _checker.Visit(VarIdExpression);

        // Assert
        Assert.Equal("Num", result2.TypeName);
    }
   
    [Fact]
    public void Visit_IdentifierExpression_IsNotDeclared_ThrowsException()
    {
        // Arrange
        var VarExpression = new ConstantExpression(1);
        var VarIdExpression = new IdentifierExpression("testVar");
        var VarType = new Type("Num");
        var node = new VariableDeclaration(VarIdExpression, VarType, VarExpression);
        _checker.Visit(node);
        var VarIdExpression2 = new IdentifierExpression("testVar2");

        // Act + Assert
        Assert.Throws<Exception>(() => _checker.Visit(VarIdExpression2));
    }

}