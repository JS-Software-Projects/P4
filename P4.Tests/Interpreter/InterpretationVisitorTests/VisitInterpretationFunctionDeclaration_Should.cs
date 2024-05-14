using System.Reflection.Metadata;
using P4.Interpreter;
using P4.Interpreter.AST.Nodes;

namespace P4.Tests.Interpreter.InterpretationVisitorTests;

public class VisitInterpretationFunctionDeclaration_Should
{
    private readonly InterpretationVisitor _visitor = new();
    
    [Fact]
    public void Visit_FunctionDeclaration_AddsFunctionToEnvironment()
    {
        // Arrange
        var returnType = new Type("Num"); 
        var functionName = new IdentifierExpression("testFunction");
        var parameters = new ParameterList(new List<ParameterNode>());
        var blockStatement = new BlockStatement();

        var functionDeclaration = new FunctionDeclaration(returnType, functionName, parameters, blockStatement);

        // Act
        var result = _visitor.Visit(functionDeclaration);
        
       
        // Assert
        Assert.Null(result);
    }
}