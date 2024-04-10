using System;
using System.Collections.Generic;
using Antlr4.Runtime;

namespace P4.Interpreter;

public class TypeChecker : EduGrammarBaseVisitor<Type>
{
    private Dictionary<string, Type> types = new Dictionary<string, Type>();
    
    public Type GetTypeFromContext(ParserRuleContext context)
    {
        // This is a placeholder implementation.
        // You'll need to adjust it based on how types are represented in your grammar.
        if (context.GetText().Equals("Num", StringComparison.OrdinalIgnoreCase))
        {
            return typeof(double);
        }
        else if (context.GetText().Equals("Bool", StringComparison.OrdinalIgnoreCase))
        {
            return typeof(bool);
        }
        else if (context.GetText().Equals("string", StringComparison.OrdinalIgnoreCase))
        {
            return typeof(string);
        }
        else
        {
            // Default or error handling
            throw new NotImplementedException("Unsupported type: " + context.GetText());
        }
    }
}