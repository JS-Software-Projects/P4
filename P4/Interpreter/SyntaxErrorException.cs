using System;

namespace P4.Interpreter;

public class SyntaxErrorException : Exception
{
    public int Line { get; }
    public int Column { get; }

    public SyntaxErrorException(string message, int line, int column) : base(message)
    {
        Line = line;
        Column = column;
    }

    public override string ToString()
    {
        return $"Syntax Error at Line {Line}, Column {Column}: {Message}";
    }
}