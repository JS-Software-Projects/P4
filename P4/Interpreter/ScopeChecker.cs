using System;
using System.Collections.Generic;

namespace P4.Interpreter;

public class ScopeChecker  : EduGrammarBaseVisitor<object>
{
    private Stack<Dictionary<string, Type>> scopes = new Stack<Dictionary<string, Type>>();
 /*
  * // Override methods to push/pop scopes and check variable declarations and references...
    public override object VisitSomeNode(EduGrammarParser.SomeNodeContext context)
    {
        // Implementation...
        return null; // Return null where you don't have a meaningful value to return.
    }
  */
}