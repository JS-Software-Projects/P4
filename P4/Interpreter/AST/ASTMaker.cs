using System;
using System.Collections.Generic;
using System.Linq;
using Antlr4.Runtime.Atn;

namespace P4.Interpreter.AST;
using P4.Interpreter;

public class ASTMaker : EduGrammarBaseVisitor<ASTNode>
{
    private int _lineNumber = 0;
    public override ASTNode VisitProgram(EduGrammarParser.ProgramContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }
        var programNode = new ProgramNode();
        if (context.children != null)
        {
            foreach (var child in context.children)
            {
                var result = Visit(child);
                if (result != null)
                {
                    programNode.AddChild(result);
                }
            }
        }
        return programNode;
    }
    public override ASTNode VisitLine(EduGrammarParser.LineContext context)
    {
        if (context == null || context.Start == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        var token = context.Start;
        _lineNumber = token.Line;
        return base.VisitLine(context);
    }
   
    public List<ParameterNode> VisitParameterList(EduGrammarParser.ParameterListContext context) {
        var parameters = new List<ParameterNode>();

        // Iterate through all parameter contexts in the parameter list
        foreach (var paramCtx in context.parameter()) {
            // Assuming each parameter context has a type and an identifier
            string typeName = paramCtx.type().GetText(); // GetText might vary based on how you define 'type'
            string paramName = paramCtx.id().GetText(); // Similarly, for 'identifier'

            // Create a new ParameterNode and add it to the list
            var parameterNode = new ParameterNode(paramName, typeName);
            parameters.Add(parameterNode);
        }

        return parameters;
    }
    
    public override ASTNode VisitFunctionDeclaration(EduGrammarParser.FunctionDeclarationContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        var typeContext = context.type();
        if (typeContext == null)
        {
            throw new ArgumentNullException(nameof(typeContext));
        }

        var idContext = context.id();
        if (idContext == null)
        {
            throw new ArgumentNullException(nameof(idContext));
        }

        var parameterListContext = context.parameterList();
        if (parameterListContext == null)
        {
            throw new ArgumentNullException(nameof(parameterListContext));
        }

        var blockContext = context.block();
        if (blockContext == null)
        {
            throw new ArgumentNullException(nameof(blockContext));
        }

        var type = VisitType(typeContext) as Type;
        if (type == null)
        {
            throw new InvalidCastException("Expected Type");
        }

        var typeName = type.TypeName;
        var name = VisitId(idContext) as IdentifierExpression; // Correctly accessing the function's name
        var parameters = VisitParameterList(parameterListContext);
        var body = VisitBlock(blockContext) as BlockStatement;

        return new FunctionDeclaration(type, name, parameters, body);
    }

    public override ASTNode VisitVariableDeclaration(EduGrammarParser.VariableDeclarationContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        var typeContext = context.type();
        if (typeContext == null)
        {
            throw new ArgumentNullException(nameof(typeContext));
        }

        var idContext = context.id();
        if (idContext == null)
        {
            throw new ArgumentNullException(nameof(idContext));
        }

        var type = typeContext.GetText();
        var variableName = idContext.GetText();

        var exprContext = context.expr();
        var expression = exprContext != null ? Visit(exprContext) as Expression : null;

        return new VariableDeclaration(variableName, type, expression);
    }

    public override ASTNode VisitConstant(EduGrammarParser.ConstantContext context)
    {
        if (context.Num() != null)
        {
            // Parse the number and create a ConstantExpression
            var value = double.Parse(context.Num().GetText());
            return new ConstantExpression(value);
        }
        else if (context.String() != null)
        {
            // Remove the quotes and create a ConstantExpression
            var value = context.String().GetText().Trim('"');
            return new ConstantExpression(value);
        }
        else if (context.Bool() != null)
        {
            // Parse the boolean and create a ConstantExpression
            var value = bool.Parse(context.Bool().GetText());
            return new ConstantExpression(value);
        }
        else // context.Null() != null
        {
            // Create a ConstantExpression with null
            return new ConstantExpression(null);
        }
    }

    public override ASTNode VisitExpr(EduGrammarParser.ExprContext context)
{
    return VisitBoolExpr(context.boolExpr());
}

public override ASTNode VisitBoolExpr(EduGrammarParser.BoolExprContext context)
{
    var left = VisitComparisonExpr(context.comparisonExpr(0)) as Expression;
    for (int i = 1; i < context.comparisonExpr().Length; i++)
    {
        var right = VisitComparisonExpr(context.comparisonExpr(i)) as Expression;
        var op = OperatorExtensions.FromString(context.boolOp(i - 1).GetText());
        left = new BinaryExpression(left, op, right);
    }
    return left;
}

public override ASTNode VisitComparisonExpr(EduGrammarParser.ComparisonExprContext context)
{
    var left = VisitAdditionExpr(context.additionExpr(0)) as Expression;
    for (int i = 1; i < context.additionExpr().Length; i++)
    {
        var right = VisitAdditionExpr(context.additionExpr(i)) as Expression;
        var op = OperatorExtensions.FromString(context.compareOp(i - 1).GetText());
        left = new BinaryExpression(left, op, right);
    }
    return left;
}

public override ASTNode VisitAdditionExpr(EduGrammarParser.AdditionExprContext context)
{
    var left = VisitMultiplicationExpr(context.multiplicationExpr(0)) as Expression;
    for (int i = 1; i < context.multiplicationExpr().Length; i++)
    {
        var right = VisitMultiplicationExpr(context.multiplicationExpr(i)) as Expression;
        var op = OperatorExtensions.FromString(context.addSubOp(i - 1).GetText());
        left = new BinaryExpression(left, op, right);
    }
    return left;
}

public override ASTNode VisitMultiplicationExpr(EduGrammarParser.MultiplicationExprContext context)
{
    var left = VisitUnaryExpr(context.unaryExpr(0)) as Expression;
    for (int i = 1; i < context.unaryExpr().Length; i++)
    {
        var right = VisitUnaryExpr(context.unaryExpr(i)) as Expression;
        var op = OperatorExtensions.FromString(context.GetChild(i).GetText());
        left = new BinaryExpression(left, op, right);
    }
    return left;
}

public override ASTNode VisitUnaryExpr(EduGrammarParser.UnaryExprContext context)
{
    if (context.unOP() == null || context.unOP().Length == 0)
    {
        return VisitTernaryExpr(context.ternaryExpr());
    }
    var opContext = context.unOP()[0]; // replace '0' with the appropriate index
    var op = OperatorExtensions.FromString(opContext.GetText());
    var right = VisitTernaryExpr(context.ternaryExpr()) as Expression;
    return new UnaryExpression(op, right);
}
public override ASTNode VisitTernaryExpr(EduGrammarParser.TernaryExprContext context)
{
    if (context.term(1) != null)
    {
        var condition = VisitTerm(context.term(0)) as Expression;
        var trueExpr = VisitTerm(context.term(1)) as Expression;
        var falseExpr = VisitTerm(context.term(2)) as Expression;
        return new TernaryExpression(condition, trueExpr, falseExpr);
    }
    return VisitTerm(context.term(0));
}

public override ASTNode VisitTerm(EduGrammarParser.TermContext context)
{
    if (context.id() != null)
    {
        return VisitId(context.id());
    }
    else if (context.constant() != null)
    {
        return VisitConstant(context.constant());
    }
    else if (context.parenExpr() != null)
    {
        return VisitParenExpr(context.parenExpr());
    }
    else if (context.functionCall() != null)
    {
        return VisitFunctionCall(context.functionCall());
    }

    return null;
}
    public override ASTNode VisitId(EduGrammarParser.IdContext context)
    {
        return new IdentifierExpression(context.GetText());
    }
    public override ASTNode VisitType(EduGrammarParser.TypeContext context)
    {
        return new Type(context.GetText());
    }
    
    public override ASTNode VisitBlock(EduGrammarParser.BlockContext context) {
        var block = new BlockStatement();
        foreach (var line in context.line()) {
            var statementNode = Visit(line);
            if (statementNode != null) {
                block.Statements.Add(statementNode as Statement);
            } else {
                // Log or handle the case where Visit(line) returns null
            }
        }
        return block;
    }
    public override ASTNode VisitPrint(EduGrammarParser.PrintContext context)
    {
        return new PrintStatement((Expression)Visit(context.expr()));
    }
    
    public override ASTNode VisitAssignment(EduGrammarParser.AssignmentContext context)
    {
        var idNode = Visit(context.id()) as IdentifierExpression;
        if (idNode == null)
        {
            throw new InvalidCastException("Expected IdentifierExpression");
        }
        var variableName = idNode.Name;
        return new AssignmentStatement(variableName, (Expression)Visit(context.expr()));
    }
    public override ASTNode VisitIfBlock(EduGrammarParser.IfBlockContext context)
    {
        var condition = (Expression)Visit(context.expr());
        var ifBlock = (BlockStatement)Visit(context.block());
        var elseBlock = context.elseBlock() != null ? (BlockStatement)Visit(context.elseBlock()) : null;

        return new IfBlock(condition, ifBlock, elseBlock);
    }
    public override ASTNode VisitElseBlock(EduGrammarParser.ElseBlockContext context)
    {
        return Visit(context.block());
    }
    public override ASTNode VisitWhileBlock(EduGrammarParser.WhileBlockContext context)
    {
        var condition = (Expression)Visit(context.expr());
        var block = (BlockStatement)Visit(context.block());

        return new WhileBlock(condition, block);
    }
    public override ASTNode VisitForLoop(EduGrammarParser.ForLoopContext context)
    {
        var init = (Statement)Visit(context.variableDeclaration());
        var condition = (Expression)Visit(context.expr());
        var update = (Statement)Visit(context.assignment());
        var block = (BlockStatement)Visit(context.block());

        return new ForLoopStatement(init, condition, update, block);
    }
    public override ASTNode VisitReturnStatement(EduGrammarParser.ReturnStatementContext context)
    {
        return new ReturnStatement((Expression)Visit(context.expr()));
    }
    public override ASTNode VisitFunctionCall(EduGrammarParser.FunctionCallContext context)
    {
        var functionName = context.id().GetText();
        var arguments = context.argumentList() != null ? VisitArgumentList(context.argumentList()) : new List<Expression>();

        return new FunctionCallStatement(functionName, arguments);
    }
    public new List<Expression> VisitArgumentList(EduGrammarParser.ArgumentListContext context)
    {
        var arguments = new List<Expression>();

        foreach (var exprCtx in context.expr())
        {
            var argument = Visit(exprCtx) as Expression;
            if (argument == null)
            {
                throw new InvalidCastException("Expected Expression");
            }
            arguments.Add(argument);
        }

        return arguments;
    }
    // Continue with other methods for other types of nodes
}
