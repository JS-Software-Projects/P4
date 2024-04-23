using Xunit;
using P4.Interpreter.AST;
using System.Text;
namespace P4.Interpreter.Tests;

public class ParameterNode_should
{
    [Fact]
    public void ParameterNode_InitializesWithCorrectValues()
    {
        // Arrange
        var node = new ParameterNode("Test", "String");
        // Act & Assert 
        Assert.Equal("String Test", node.ToString());
    }

    [Fact]
    public void ParameterNode_InitializesWithEmptyValues()
    {
        // Arrange
        var node = new ParameterNode("", "");
        // Act & Assert
        Assert.Equal(" ", node.ToString());
    }

    [Fact]
    public void ParameterNode_InitializesWithNullValues()
    {
        // Arrange
        var node = new ParameterNode(null, null);
        // Act & Assert
        Assert.Equal(" ", node.ToString());
    }
}