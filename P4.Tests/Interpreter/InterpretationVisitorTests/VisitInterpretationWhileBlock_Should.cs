using Microsoft.VisualBasic.CompilerServices;
using P4.Interpreter;
using P4.Interpreter.AST.Nodes;

namespace P4.Tests.Interpreter.InterpretationVisitorTests;

public class VisitInterpretationWhileBlock_Should
{
    private readonly InterpretationVisitor _visitor = new();

    [Fact]
    public void WhileBlock_Constructor_SetsPropertiesCorrectly()
    {
        // Arrange
        var declaration = new VariableDeclaration(new IdentifierExpression("x"), new Type("Num"), new ConstantExpression(1.0));
        _visitor.Visit(declaration);
        var variable = new IdentifierExpression("x");
        var add = new BinaryExpression(variable, Operator.Add, new ConstantExpression(1.0));
        var lessThan = new BinaryExpression(new IdentifierExpression("x"), Operator.LessThan, new ConstantExpression(2.0));
        
            
        var condition = new ConstantExpression(lessThan);
        var block = new BlockStatement();
        block.Statements.Add(new AssignmentStatement(new IdentifierExpression("x"), add));
        var whileBlock = new WhileBlock(condition, block);
        
        // Act
        _visitor.Visit(whileBlock);
        var result = _visitor.Visit(variable);
        
        // Assert
        Assert.Equal(2.0,result);

    }
    
}