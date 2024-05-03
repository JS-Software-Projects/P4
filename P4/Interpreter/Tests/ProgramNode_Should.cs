using Xunit;
using P4.Interpreter.AST;
using System.Text;
namespace P4.Interpreter.Tests;

public class ProgramNode_Should
{
    [Fact]
    public void ProgramNode_InitializesEmptyChildrenList()
    {
        // Arrange
        var node = new ProgramNode();
        // Act & Assert
        Assert.Empty(node.GetChildren());
    }

    [Fact]
    public void AddChild_AddsChildToChildrenList()
    {
        // Arrange
        var node = new ProgramNode();
        var child = new ProgramNode();
        // Act
        node.AddChild(child);
        // Assert
        Assert.Contains(child, node.GetChildren());
    }

    [Fact]
    public void ToString_ReturnsCorrectStringWithNoChildren()
    {
        // Arrange
        var node = new ProgramNode();
        // Act
        var result = node.ToString();
        // Assert
        Assert.Equal("AST:\r\n", result);
    }

    [Fact]
    public void ToString_ReturnsCorrectStringWithChildren()
    {
        // Arrange
        var node = new ProgramNode();
        var child = new ProgramNode();
        node.AddChild(child);
        // Act
        var result = node.ToString();
        // Assert
        Assert.Equal("AST:\r\n" + child.ToString() + "\r\n", result);
    }
}