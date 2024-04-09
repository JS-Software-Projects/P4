using System;
using System.Collections.Generic;

namespace P4.Interpreter;

public class TypeChecker : EduGrammarBaseVisitor<Type>
{
    private Dictionary<string, Type> types = new Dictionary<string, Type>();
    
}