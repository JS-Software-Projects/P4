using Xunit;
using System;
using System.IO;
using Antlr4.Runtime;
using P4.Interpreter;
using P4.Interpreter.AST;
namespace P4.Tests.IntegrationTests;

public class InterpreterIntegrationTest
{
    [Fact]
    public void TestParseTreeAndASTCreation()
    {
        // Arrange
        var testFilePath = "../../../TestFiles/TestFile1.txt";
        var expectedAST = "AST:\r\nVariableDeclaration: Num IdentifierExpression: Name is = x = ConstantExpression: is = 3\r\nAssignmentStatement: IdentifierExpression: Name is = x = (IdentifierExpression: Name is = x Add ConstantExpression: is = 7)\r\n";

        // Act
        var input = new AntlrInputStream(File.ReadAllText(testFilePath));
        var lexer = new EduGrammarLexer(input);
        var tokens = new CommonTokenStream(lexer);
        var parser = new EduGrammarParser(tokens);
        var parseTree = parser.program();

        var astMaker = new ASTMaker();
        var actualAST = astMaker.VisitProgram(parseTree);

        // Assert
        Assert.Equal(expectedAST, actualAST.ToString());
    }
    [Fact]
    public void Testfile2ParsesAndASTisMade_ScopeTypeCheckerThrowsCorrectErrorMessage()
    {
        // Arrange
        var testFilePath = "../../../TestFiles/TestFile2.txt";

        // Act
        var input = new AntlrInputStream(File.ReadAllText(testFilePath));
        var lexer = new EduGrammarLexer(input);
        var tokens = new CommonTokenStream(lexer);
        var parser = new EduGrammarParser(tokens);
        var parseTree = parser.program();

        var astMaker = new ASTMaker();
        var AST = astMaker.VisitProgram(parseTree);

        var scopeTypeChecker = new ScopeTypeChecker();
        

        // Assert
        var exception = Assert.Throws<Exception>(() => scopeTypeChecker.Visit(AST));
        Assert.Equal("Type mismatch for variable 'IdentifierExpression: Name is = x'. In line:1", exception.Message);
    }
    [Fact]
    public void TestParseTreeASTCreationAndInterpretationVisitor_ReturnsNullSuccesfully()
    {
        // Arrange
        var testFilePath = "../../../TestFiles/TestFile1.txt";

        // Act
        var input = new AntlrInputStream(File.ReadAllText(testFilePath));
        var lexer = new EduGrammarLexer(input);
        var tokens = new CommonTokenStream(lexer);
        var parser = new EduGrammarParser(tokens);
        var parseTree = parser.program();

        var astMaker = new ASTMaker();
        var AST = astMaker.VisitProgram(parseTree);

        var interpretationVisitor = new InterpretationVisitor();

        // Assert
        Assert.Null(interpretationVisitor.Visit(AST));
    }
}