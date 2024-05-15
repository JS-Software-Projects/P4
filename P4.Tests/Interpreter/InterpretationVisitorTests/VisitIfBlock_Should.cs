using P4.Interpreter;
using P4.Interpreter.AST.Nodes;

namespace P4.Tests.Interpreter.InterpretationVisitorTests;

public class VisitIfBlock_Should
{
    private readonly InterpretationVisitor _visitor = new();
    
    [Fact]
    public void VisitIfBlockWithTrueCondition_VisistIfBlock()
    {
        var VarDec = new VariableDeclaration(new IdentifierExpression("x"), new Type("Num"), new ConstantExpression(0.0));
        _visitor.Visit(VarDec);
        var variableName = new IdentifierExpression("x");
        var variableValue1 = new ConstantExpression(10.0);
        var variableValue2 = new ConstantExpression(20.0);
        var assignment1 = new AssignmentStatement(variableName, variableValue1);
        var assignment2 = new AssignmentStatement(variableName, variableValue2);
        var condition = new ConstantExpression(true);
        var assignmentList1 = new List<Statement>();
        assignmentList1.Add(assignment1);
        var assignmentList2 = new List<Statement>();
        assignmentList2.Add(assignment2);
        var block = new BlockStatement();
        block.Statements = assignmentList1;
        var elseBlock = new BlockStatement();
        elseBlock.Statements = assignmentList2;
        var node = new IfBlock(condition, block, elseBlock);
        
        _visitor.Visit(node);
        var result = _visitor.Visit(variableName);
        
        Assert.Equal(10.0, result);
    }
    [Fact]
    public void VisitIfBlockWithFalseCondition_VisistElseBlock()
    {
        var VarDec = new VariableDeclaration(new IdentifierExpression("x"), new Type("Num"), new ConstantExpression(0.0));
        _visitor.Visit(VarDec);
        var variableName = new IdentifierExpression("x");
        var variableValue1 = new ConstantExpression(10.0);
        var variableValue2 = new ConstantExpression(20.0);
        var assignment1 = new AssignmentStatement(variableName, variableValue1);
        var assignment2 = new AssignmentStatement(variableName, variableValue2);
        var condition = new ConstantExpression(false);
        var assignmentList1 = new List<Statement>();
        assignmentList1.Add(assignment1);
        var assignmentList2 = new List<Statement>();
        assignmentList2.Add(assignment2);
        var block = new BlockStatement();
        block.Statements = assignmentList1;
        var elseBlock = new BlockStatement();
        elseBlock.Statements = assignmentList2;
        var node = new IfBlock(condition, block, elseBlock);
        
        _visitor.Visit(node);
        var result = _visitor.Visit(variableName);
        
        Assert.Equal(20.0, result);
    }
    [Fact]
    public void VisitIfBlockWithFalseConditionAndElseBlockIsNull()
    {
        var variableName = new IdentifierExpression("x");
        var variableValue1 = new ConstantExpression(10.0);
        var assignment1 = new AssignmentStatement(variableName, variableValue1);
        var condition = new ConstantExpression(false);
        var assignmentList1 = new List<Statement>();
        assignmentList1.Add(assignment1);
        var assignmentList2 = new List<Statement>();
        var block = new BlockStatement();
        block.Statements = assignmentList1;
        var elseBlock = new BlockStatement();
        elseBlock.Statements = assignmentList2;
        var node = new IfBlock(condition, block, elseBlock);
        
        var result = _visitor.Visit(node);
      
        
        Assert.Equal(null, result);
    }
}