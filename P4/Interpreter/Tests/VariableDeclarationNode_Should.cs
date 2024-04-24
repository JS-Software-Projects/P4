using Xunit;
using P4.Interpreter.AST;

public class VariableDeclarationNode_Should
{
    [Fact]
    public void VariableDeclarationNode_WithNonNullExpression_ReturnsCorrectString()
    {
        // Arrange
        var expression = new ConstantNode(5);
        var type = new TypeNode("Num");
        var id = new IdentifierNode("x");
        var node = new VariableDeclarationNode(type, id, expression);

        // Act
        var result = node.ToString();

        // Assert
        Assert.Equal("Variable Declaration: x = 5", result);
    }

    [Fact]
    public void VariableDeclarationNode_WithNullExpression_ReturnsCorrectString()
    {
        // Arrange
        var type = new TypeNode("Num");
        var id = new IdentifierNode("x");
        var node = new VariableDeclarationNode(type, id, null);

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
        var type = new TypeNode("String");
        var id = new IdentifierNode("x");
        var node = new VariableDeclarationNode(type, id, expression);

        // Act
        var result = node.ToString();

        // Assert
        Assert.Equal("Variable Declaration: x = hej", result);
    }
}