using Xunit;
using P4.Interpreter.AST;
using System.Text;
namespace P4.Tests.Interpreter;
using P4.Interpreter.AST.Nodes;
public class ParameterNode_should
{
    [Fact]
    public void ParameterNode_InitializesWithCorrectValues()
    {
        // Arrange
        var VarIdExpression = new IdentifierExpression("test");
        var VarType = new Type("Num");
        var node = new ParameterNode(VarIdExpression, VarType);
        // Act & Assert 
        Assert.Equal(VarType.TypeName + " " + VarIdExpression.ToString(), node.ToString());
    }

    [Fact]
    public void ParameterNode_InitializesWithEmptyValues()
    {
        // Arrange
        var VarIdExpression = new IdentifierExpression("");
        var VarType = new Type("");
        var node = new ParameterNode(VarIdExpression, VarType);
        // Act & Assert
        Assert.Equal(" " + VarIdExpression, node.ToString());
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