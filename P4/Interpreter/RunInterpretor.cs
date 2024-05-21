using System;
using System.IO;
using Antlr4.Runtime;
using P4.Interpreter.AST;
namespace P4.Interpreter;

public class RunInterpretor
{
    public static void Execute(string fileName)
    {
        try
        {
            var input = new AntlrInputStream(File.ReadAllText(fileName));
            var lexer = new EduGrammarLexer(input);
            var tokens = new CommonTokenStream(lexer);
            var parser = new EduGrammarParser(tokens);
            var parseTree = parser.program();
            
            Console.WriteLine("Parse tree:");
            Console.WriteLine(parseTree.ToStringTree(parser)+"\n");
            
            
            var astMaker = new ASTMaker();
            var AST = astMaker.VisitProgram(parseTree);
            
            var scopeTypeChecker = new ScopeTypeChecker();
            scopeTypeChecker.Visit(AST);
            Console.WriteLine(AST.ToString());
            var interpretationVisitor = new InterpretationVisitor();
            interpretationVisitor.Visit(AST);
            
            /*
            Console.WriteLine("\n");
            Console.WriteLine("                       ProgramNode      ");
            Console.WriteLine("              /                         \\");
            Console.WriteLine("   VariableDeclarationNode                 AssignmentNode");
            Console.WriteLine("      /      |           \\                     /            \\");
            Console.WriteLine("TypeNode  IdentifierNode  ConstantNode    IdentifierNode  ExpressionNode ");
            Console.WriteLine("                                                               /       \\");
            Console.WriteLine("                                                        variableNode  ConstantNode");
            Console.WriteLine("\n");  
            Console.WriteLine("                  ProgramNode      ");
            Console.WriteLine("              /                \\");
            Console.WriteLine("       Num   x  =  10         x = x + 2");
            Console.WriteLine("      /      |      \\        /      |   ");
            Console.WriteLine("     Num     x       10      x    x + 2");
            Console.WriteLine("                                 /     \\ ");
            Console.WriteLine("                                x       2");
            */
            
            /*
            var scopeChecker = new ScopeChecker();
            scopeChecker.Visit(tree);
            var typeChecker = new TypeChecker();
            typeChecker.Visit(tree);
            var visitor = new EduVisitor();
            visitor.Visit(tree);
            */
            
        } catch (Exception e)
        {
            Terminal.SetError(true, e.Message);
            Console.WriteLine("Error:"+e.Message);
            Console.WriteLine("Error: "+e.StackTrace);
        }
    }
}