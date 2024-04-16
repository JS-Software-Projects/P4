using System;
using System.Collections.Generic;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace P4.Interpreter.Tests;

[TestFixture]
public class VisistExprTest
{
    private EduGrammarParser Setup(string text)
    {
        AntlrInputStream inputStream = new AntlrInputStream(text);
        var lexer = new EduGrammarLexer(inputStream);
        var tokens = new CommonTokenStream(lexer);
        var parser = new EduGrammarParser(tokens);
        return parser;   
    }
    
    [Test] 
    public void VisitIdExpr_VariableExists_ReturnsValue()
    {
        // Arrange
        var parser = Setup("testVar");
        var visitor = new EduVisitor();
        visitor.AddVariableForTesting("testVar", 42); // Add variable directly

        // Act
        var context = parser.expr();
        var result = visitor.Visit(context);

        // Assert
        // Assert.That(42, Is.EqualTo(result));
         ClassicAssert.AreEqual(42, result);
    }
    [Test]
    public void VisitIdExpr_VariableDoesNotExist_ThrowsException()
    {
        // Arrange
        var parser = Setup("missingVar");
        var visitor = new EduVisitor(); // No variables added
        var contextMock = new Mock<EduGrammarParser.IdentifierContext>();

        // Act
        var context = parser.expr();

        // Assert
        var ex = Assert.Throws<Exception>(() => visitor.Visit(context));
        Assert.That(ex.Message, Is.EqualTo("Variable missingVar not found at line: 0"));
    }
}