using Xunit;
using P4.Interpreter.AST;

public class VariableDeclarationNode_Should
{
    [Fact]
    public void VariableDeclarationNode_WithNonNullExpression_ReturnsCorrectString()
    {
        // Arrange
        var expression = new ConstantNode(5);
        var node = new VariableDeclarationNode("int", "x", expression);

        // Act
        var result = node.ToString();

        // Assert
        Assert.Equal("Variable Declaration: x = 5", result);
    }

    [Fact]
    public void VariableDeclarationNode_WithNullExpression_ReturnsCorrectString()
    {
        // Arrange
        var node = new VariableDeclarationNode("int", "x", null);

        // Act
        var result = node.ToString();

        // Assert
        Assert.Equal("Variable Declaration: x", result);
    }
    
    [Fact]
    public void VariableDeclarationNode_WithNonNullExpressionAndString_ReturnsCorrectString()
    {
        // Arrange
        var expression = new ConstantNode("hej");
        var node = new VariableDeclarationNode("String", "x", expression);

        // Act
        var result = node.ToString();

        // Assert
        Assert.Equal("Variable Declaration: x = hej", result);
    }
}