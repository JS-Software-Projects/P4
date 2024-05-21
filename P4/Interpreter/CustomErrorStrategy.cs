using System.Data;
using Antlr4.Runtime;
namespace P4.Interpreter;

public class CustomErrorStrategy : DefaultErrorStrategy
{
    public override void Recover(Parser recognizer, RecognitionException e)
    {
        throw new SyntaxErrorException(e.Message, recognizer.CurrentToken.Line, recognizer.CurrentToken.Column);
    }

    protected override void ReportMissingToken(Parser recognizer)
    {
        var token = recognizer.CurrentToken;
        var errorMessage = $"Missing token at line {token.Line}, column {token.Column}";
        throw new SyntaxErrorException(errorMessage, token.Line, token.Column);
    }

    protected override void ReportUnwantedToken(Parser recognizer)
    {
        var token = recognizer.CurrentToken;
        var errorMessage = $"Unwanted token '{token.Text}' at line {token.Line}, column {token.Column}";
        throw new SyntaxErrorException(errorMessage, token.Line, token.Column);
    }

    protected override void ReportInputMismatch(Parser recognizer, InputMismatchException e)
    {
        var token = recognizer.CurrentToken;
        var errorMessage = $"Input mismatch '{token.Text}' at line {token.Line}, column {token.Column}";
        throw new SyntaxErrorException(errorMessage, token.Line, token.Column);
    }

    public override void ReportError(Parser recognizer, RecognitionException e)
    {
        // Do not generate any error messages
    }
}