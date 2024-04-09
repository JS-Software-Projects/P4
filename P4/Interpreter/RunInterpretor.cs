using System;
using System.IO;
using Antlr4.Runtime;

namespace P4.Interpreter;

public class RunInterpretor
{
    public static void Execute()
    {
        try
        {
            var fileName = "../../../Output/output.txt";

            var input = new AntlrInputStream(File.ReadAllText(fileName));
            var lexer = new EduGrammarLexer(input);
            var tokens = new CommonTokenStream(lexer);
            var parser = new EduGrammarParser(tokens);
            var tree = parser.program();
            
            var scopeChecker = new ScopeChecker();
            scopeChecker.Visit(tree);
            
            var typeChecker = new TypeChecker();
            typeChecker.Visit(tree);
            
            var visitor = new EduVisitor();
            visitor.Visit(tree);
        } catch (Exception e)
        {
            Terminal.SetError(true, e.Message);
            Console.WriteLine("Error: "+e.Message);
        }
    }
}