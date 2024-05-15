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
        // 
        var variable = new IdentifierExpression("x");
        var declaration = new VariableDeclaration(variable, new Type("Num"), new ConstantExpression(1.0));
        _visitor.Visit(declaration);
        
        var add = new BinaryExpression(variable, Operator.Add, new ConstantExpression(1.0));
        var condition = new BinaryExpression(variable, Operator.LessThan, new ConstantExpression(2.0));
        
            
    
        var block = new BlockStatement();
        block.Statements.Add(new AssignmentStatement(variable, add));
        var whileBlock = new WhileBlock(condition, block);
        
        // Act
        _visitor.Visit(whileBlock);
        var result = _visitor.Visit(variable);
        
        // Assert
        Assert.Equal(2.0,result);

    }
    
}