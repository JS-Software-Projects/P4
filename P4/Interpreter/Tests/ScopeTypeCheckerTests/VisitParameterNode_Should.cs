namespace P4.Interpreter.Tests.ScopeTypeCheckerTests;
using P4.Interpreter.AST;
using Xunit;

public class VisitParameterNode_Should
{
    private readonly ScopeTypeChecker _checker = new();
    
    [Fact]
    public void Visit_ParameterNode_ReturnsType_Success()
        {
            // Arrange
           
            var VarIdExpression = new IdentifierExpression("testVar");
            var VarType = new Type("Num");
            var node = new ParameterNode(VarIdExpression, VarType);
            
            // Act
            var result = _checker.Visit(node);
            
            // Assert
            Assert.Equal("Num", result.TypeName);
        }
    
    
}