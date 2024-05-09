using P4.Interpreter.AST;
using Xunit;
using System;

namespace P4.Interpreter.Tests.ScopeTypeCheckerTests;

public class VisitVariableDeclaration_Should
{
    private readonly ScopeTypeChecker _checker = new();
    [Fact]
    public void Visit_VariableDeclaration_ReturnsNullWhenSuccessful()
    {
        // Arrange
        var VarExpression = new ConstantExpression(1);
        var VarIdExpression = new IdentifierExpression("testVar");
        var VarType = new Type("Num");
        var node = new VariableDeclaration(VarIdExpression, VarType, VarExpression);

        // Act
        var result = _checker.Visit(node);

        // Assert
        Assert.Equal(null, result);
    }
    
    [Fact]
    public void Visit_VariableDeclaration_IsDeclaredInScope_ThrowsException()
    {
        // Arrange
        var VarExpression = new ConstantExpression(1);
        var VarIdExpression = new IdentifierExpression("testVar");
        var VarType = new Type("Num");
        var node = new VariableDeclaration(VarIdExpression, VarType, VarExpression);
        var node2 = new VariableDeclaration(VarIdExpression, VarType, VarExpression);
        _checker.Visit(node);
        
        
        // Act
        
       
        // Assert
        Assert.Throws<Exception>(() => _checker.Visit(node2));
    }
    
    [Fact]
    public void Visit_VariableDeclaration_IsNotDeclared_Success()
    {
        // Arrange
        var VarExpression = new ConstantExpression(1);
        var VarIdExpression = new IdentifierExpression("testVar");
        var VarIdExpression2 = new IdentifierExpression("testVar2");
        var VarType = new Type("Num");
        var node = new VariableDeclaration(VarIdExpression, VarType, VarExpression);
        var node2 = new VariableDeclaration(VarIdExpression2, VarType, VarExpression);

        // Act
         _checker.Visit(node);
        var result2 = _checker.Visit(node2);
       
        // Assert
        Assert.Equal(null, result2);
    }
    
    
    [Fact]
    public void Visit_VariableDeclaration_IsNotCorrectType_ThrowsException()
    {
        // Arrange
        var VarExpression = new ConstantExpression(1);
        var VarIdExpression = new IdentifierExpression("testVar");
        var VarType = new Type("Unknown");
        var node = new VariableDeclaration(VarIdExpression, VarType, VarExpression);

        // Act in visit+Assert in the throws
        Assert.Throws<Exception>(() => _checker.Visit(node));
    }
    
    
    [Fact]
    public void Visit_VariableDeclaration_IsNotTypeCorrect_ThrowsException()
    {
        // Arrange
        var VarExpression = new ConstantExpression("Hej");
        var VarIdExpression = new IdentifierExpression("testVar");
        var VarType = new Type("Num");
        var node = new VariableDeclaration(VarIdExpression, VarType, VarExpression);
        
        // Act
       // var result = _checker.Visit(node);
       
        // Assert
        Assert.Throws<Exception>(() => _checker.Visit(node));
    }
    
}