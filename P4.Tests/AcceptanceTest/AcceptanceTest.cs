using Antlr4.Runtime;
using P4.Interpreter;
using P4.Interpreter.AST;
using P4.Interpreter.AST.Nodes;

namespace P4.Tests.AcceptanceTest;

public class AcceptanceTest
{
    [Fact]
    public void TestParseTreeASTCreationAndInterpretationVisitor_Returns12Succesfully()
    {
        // Arrange
        var testFilePath = "../../../TestFiles/AcceptanceTestFile.txt";

        // Act
        var input = new AntlrInputStream(File.ReadAllText(testFilePath));
        var lexer = new EduGrammarLexer(input);
        var tokens = new CommonTokenStream(lexer);
        var parser = new EduGrammarParser(tokens);
        var parseTree = parser.program();

        var astMaker = new ASTMaker();
        var AST = astMaker.VisitProgram(parseTree);
    
        var scopeTypeChecker = new ScopeTypeChecker();
        scopeTypeChecker.Visit(AST);
        var interpretationVisitor = new InterpretationVisitor();
        interpretationVisitor.Visit(AST);

        // Get the value of x from the environment
        var xValue = interpretationVisitor.Visit(new IdentifierExpression ("x"));

        // Assert
        Assert.Equal(13.0, xValue);
    }
}