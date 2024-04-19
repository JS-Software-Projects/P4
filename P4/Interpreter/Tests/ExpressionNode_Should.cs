using System;
using Xunit;
using P4.Interpreter.AST;

public class ExpressionNode_Should
{
    [Fact]
    public void ExpressionNode_WithNonNullOperands_ReturnsCorrectString()
    {
        //Arrange
        var left = new ConstantNode(5);
        var right = new ConstantNode(3);
        var node = new ExpressionNode("+", left, right);
        
        //Act
        var result = node.ToString();
        
        // Assert
        Assert.Equal("(5 + 3)", result);
    }
    
    [Fact]
    public void ExpressionNode_WithNullOperands_ReturnsCorrectString()
    {
        //Arrange //Act //Assert
        Assert.Throws<ArgumentNullException>(()=>new ExpressionNode("+", null, null));
    }
    
    [Fact]
    public void ExpressionNode_WithNullOperands_ReturnsCorrectStringRENAME()
    {
        //Arrange
        string expectedmessage = "Value cannot be null.";
        
        // Act //Assert
        var ex = Assert.Throws<ArgumentNullException>(()=>new ExpressionNode("+", null, null));
        Assert.Equal(expectedmessage, ex.Message);
    }
}