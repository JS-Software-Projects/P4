using P4.Interpreter;
using P4.Interpreter.AST.Nodes;

namespace P4.Tests.Interpreter.InterpretationVisitorTests;

public class VisitVariableDeclarationShould
{
    private readonly InterpretationVisitor _visitor = new();

    [Fact]
    public void AssignsDefaultValueToNumTypeVariableWhenNoExpressionProvided()
    {
        var node = new VariableDeclaration(new IdentifierExpression("x"), new Type("Num"), null);
        _visitor.Visit(node);
        var result = _visitor.Visit(new IdentifierExpression("x"));
        Assert.Equal(0.0, result);
    }

    [Fact]
    public void AssignsDefaultValueToStringTypeVariableWhenNoExpressionProvided()
    {
        var node = new VariableDeclaration(new IdentifierExpression("x"), new Type("String"), null);
        _visitor.Visit(node);
        var result = _visitor.Visit(new IdentifierExpression("x"));
        Assert.Equal(" ", result);
    }

    [Fact]
    public void AssignsDefaultValueToBoolTypeVariableWhenNoExpressionProvided()
    {
        var node = new VariableDeclaration(new IdentifierExpression("x"), new Type("Bool"), null);
        _visitor.Visit(node);
        var result = _visitor.Visit(new IdentifierExpression("x"));
        Assert.Equal(false, result);
    }

    [Fact]
    public void ThrowsExceptionWhenUnsupportedVariableTypeProvided()
    {
        var node = new VariableDeclaration(new IdentifierExpression("x"), new Type("UnsupportedType"), null);
        Assert.Throws<Exception>(() => _visitor.Visit(node));
    }

    [Fact]
    public void AssignsValueToVariableWhenExpressionProvided()
    {
        var node = new VariableDeclaration(new IdentifierExpression("x"), new Type("Num"),
            new ConstantExpression(10.0));
        _visitor.Visit(node);
        var result = _visitor.Visit(new IdentifierExpression("x"));
        Assert.Equal(10.0, result);
    }
}