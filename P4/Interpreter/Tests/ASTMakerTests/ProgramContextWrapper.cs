using System.Collections.Generic;
using Antlr4.Runtime.Tree;
using System.Linq;

namespace P4.Interpreter.Tests.ASTMakerTests;

public class ProgramContextWrapper
{
    private readonly EduGrammarParser.ProgramContext _context;

    public ProgramContextWrapper(EduGrammarParser.ProgramContext context)
    {
        _context = context;
    }

    public IReadOnlyList<IParseTree> Children => _context.children.ToList().AsReadOnly();
}