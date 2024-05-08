using System;
using System.Collections.Generic;
using Xunit;
using P4.Interpreter.AST;
using System.Text;
using Antlr4.Runtime.Tree;
using Moq;

namespace P4.Interpreter.Tests.ASTMakerTests;

/*public class VisitProgram_should
{

 
    [Fact]
    public void VisitProgram_ShouldReturnProgramNode_WhenContextIsNotNull()
    {
        // Arrange
        var mockChild = new Mock<IParseTree>();
        var mockChildren = new List<IParseTree> { mockChild.Object };

        var mockContext = new Mock<EduGrammarParser.ProgramContext>();
        mockContext.Setup(c => c.children).Returns(mockChildren);

        var astMaker = new ASTMaker();

        // Act
        var result = astMaker.VisitProgram(mockContext.Object);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<ProgramNode>(result);
    }

    [Fact]
    public void VisitProgram_ShouldThrowArgumentNullException_WhenContextIsNull()
    {
        // Arrange
        var astMaker = new ASTMaker();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => astMaker.VisitProgram(null));
    }
}*/