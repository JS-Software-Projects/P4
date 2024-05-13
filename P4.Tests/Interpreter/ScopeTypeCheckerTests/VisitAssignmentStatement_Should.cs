using P4.Interpreter;
using P4.Interpreter.AST.Nodes;

namespace P4.Tests.Interpreter.ScopeTypeCheckerTests;

public class VisitAssignmentStatement_Should
{
    private readonly ScopeTypeChecker _checker = new();
    
    [Fact]
    public void VisitAssignmentStatement_VariableNotDeclared_ThrowsException()
    {
     
        // Arrange    
        var varName = new IdentifierExpression("x");
        var varExpression = new ConstantExpression(1);
    
        var node = new AssignmentStatement(varName,varExpression);

        //Act+Assert
        Assert.Throws<Exception>(() => _checker.Visit(node));
    }
    
    [Fact]
    public void VisitAssignmentStatement_VariableIsDeclared_Succes()
    {
     
        // Arrange    
        var varName = new IdentifierExpression("x");
        var varExpression = new ConstantExpression(1);
        var varType = new Type("Num");
        var node2 = new VariableDeclaration(varName,varType, varExpression);
        _checker.Visit(node2); //Node is added to symboltable.
        var node = new AssignmentStatement(varName,varExpression);
        
        //Act
        var result = _checker.Visit(node);
        //Assert
        Assert.Null(result);
    }
    
    [Fact]
    public void VisitAssignmentStatement_VariableWrongType_ThrowsException()
    {
        // Arrange    
        var varName = new IdentifierExpression("x");
        var varExpression = new ConstantExpression(10);
        var varType = new Type("Num");
        var node2 = new VariableDeclaration(varName,varType, varExpression);
        _checker.Visit(node2); //Node is added to symboltable.
        var varExpression2 = new ConstantExpression("testVar");
        var node = new AssignmentStatement(varName,varExpression2);
        
        //Act
        
        //Assert
        Assert.Throws<Exception>(() => _checker.Visit(node));
    }
    
}