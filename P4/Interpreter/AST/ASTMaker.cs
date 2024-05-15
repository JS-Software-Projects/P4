using System;
using System.Collections.Generic;
using System.Linq;
using Antlr4.Runtime.Atn;
using P4.Interpreter.AST.Nodes;

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
    public override ASTNode VisitStatement(EduGrammarParser.StatementContext context)
    {
        var token = context.Start;
        _lineNumber = token.Line;
        return base.VisitStatement(context);
    }
    public override ASTNode VisitGameObjectDeclaration(EduGrammarParser.GameObjectDeclarationContext context)
    {
        var classType = (TypeClass)VisitGameType(context.gameType()[0]);
        var objectName = VisitId(context.id()) as IdentifierExpression;
        var arguments = context.argumentList() != null ? VisitArgumentList(context.argumentList()) as ArgumentList : new ArgumentList();

        return new GameObjectDeclaration(classType, objectName, arguments)
        {
            LineNumber = _lineNumber
        };
    }
    public override ASTNode VisitGameObjectMethodCall(EduGrammarParser.GameObjectMethodCallContext context)
    {
        var objectName = VisitId(context.id()) as IdentifierExpression;
        var arguments = context.argumentList() != null ? VisitArgumentList(context.argumentList()) as ArgumentList : new ArgumentList();
        var methodName = context.ID().GetText();
        return new GameObjectCall(objectName,methodName, arguments)
        {
            LineNumber = _lineNumber
        };
    }
    public override ASTNode VisitGameType(EduGrammarParser.GameTypeContext context)
    {
        return new TypeClass(context.GetText())
        {
            LineNumber = _lineNumber
        };
    }
   
    public override ASTNode VisitParameterList(EduGrammarParser.ParameterListContext context) {
        var parameters = new List<ParameterNode>();

        // Iterate through all parameter contexts in the parameter list
        foreach (var paramCtx in context.parameter()) {
            // Assuming each parameter context has a type and an identifier
            var typeName = VisitType(paramCtx.type()) as Type; // GetText might vary based on how you define 'type'
            var paramName = VisitId(paramCtx.id()) as IdentifierExpression; // Similarly, for 'identifier'

            // Create a new ParameterNode and add it to the list
            var parameterNode = new ParameterNode(paramName, typeName)
            {
                LineNumber = _lineNumber
            };
            parameters.Add(parameterNode);
        }

        return  new ParameterList(parameters);
    }
    
    public override ASTNode VisitFunctionDeclaration(EduGrammarParser.FunctionDeclarationContext context)
    {
        var token = context.Start;
        _lineNumber = token.Line;
        var type = VisitType(context.type()) as Type;
        var name = VisitId(context.id()) as IdentifierExpression; // Correctly accessing the function's name
        var parameters = VisitParameterList(context.parameterList()) as ParameterList;
        var body = VisitBlock(context.block()) as BlockStatement;
        return new FunctionDeclaration(type, name, parameters, body)
        {
            LineNumber = _lineNumber
        };
    }

    public override ASTNode VisitVariableDeclaration(EduGrammarParser.VariableDeclarationContext context)
    {
        var type = VisitType(context.type()) as Type;
        var variableName = VisitId(context.id()) as IdentifierExpression;

        var exprContext = context.expr();
        var expression = exprContext != null ? Visit(exprContext) as Expression : null;

        return new VariableDeclaration(variableName, type, expression)
        {
            LineNumber = _lineNumber
        };
    }

    public override ASTNode VisitConstant(EduGrammarParser.ConstantContext context)
    {
        if (context.Num() != null)
        {
            // Parse the number and create a ConstantExpression
            var value = double.Parse(context.Num().GetText());
            return new ConstantExpression(value)
            {
                LineNumber = _lineNumber
            };
        }
        else if (context.String() != null)
        {
            // Remove the quotes and create a ConstantExpression
            var value = context.String().GetText().Trim('"');
            return new ConstantExpression(value)
            {
                LineNumber = _lineNumber
            };
        }
        else if (context.Bool() != null)
        {
            // Parse the boolean and create a ConstantExpression
            var value = bool.Parse(context.Bool().GetText());
            return new ConstantExpression(value)
            {
                LineNumber = _lineNumber
            };
            
        }
        else // context.Null() != null
        {
            // Create a ConstantExpression with null
            return new ConstantExpression(null)
            {
                LineNumber = _lineNumber
            };
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
        left = new BinaryExpression(left, op, right)
        {
            LineNumber = _lineNumber
        };
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
        left = new BinaryExpression(left, op, right)
        {
            LineNumber = _lineNumber
        };
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
        left = new BinaryExpression(left, op, right)
        {
            LineNumber = _lineNumber
        };
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
        left = new BinaryExpression(left, op, right)
        {
            LineNumber = _lineNumber
        };
    }
    return left;
}

public override ASTNode VisitUnaryExpr(EduGrammarParser.UnaryExprContext context)
{
    if (context.unOP().Length == 0)
    {
        return VisitTernaryExpr(context.ternaryExpr());
    }
    var opContext = context.unOP()[0]; 
    var op = OperatorExtensions.FromString(opContext.GetText());
    var right = VisitTernaryExpr(context.ternaryExpr()) as Expression;
    return new UnaryExpression(op, right)
    {
        LineNumber = _lineNumber
    };
}
public override ASTNode VisitTernaryExpr(EduGrammarParser.TernaryExprContext context)
{
    if (context.term(1) != null)
    {
        var condition = VisitTerm(context.term(0)) as Expression;
        var trueExpr = VisitTerm(context.term(1)) as Expression;
        var falseExpr = VisitTerm(context.term(2)) as Expression;
        return new TernaryExpression(condition, trueExpr, falseExpr)
        {
            LineNumber = _lineNumber
        };
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
        return new IdentifierExpression(context.GetText())
        {
            LineNumber = _lineNumber
        };
    }
    public override ASTNode VisitType(EduGrammarParser.TypeContext context)
    {
        return new Type(context.GetText())
        {
            LineNumber = _lineNumber
        };
    }
    
    public override ASTNode VisitBlock(EduGrammarParser.BlockContext context)
    {
        var block = new BlockStatement();
        foreach (var statement in context.statement()) {
            var statementNode = Visit(statement);
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
        return new PrintStatement((Expression)Visit(context.expr()))
        {
            LineNumber = _lineNumber
        };
    }
    
    public override ASTNode VisitAssignment(EduGrammarParser.AssignmentContext context)
    {
        var idNode = Visit(context.id()) as IdentifierExpression;
        if (idNode == null)
        {
            throw new InvalidCastException("Expected IdentifierExpression");
        }

        return new AssignmentStatement(idNode, (Expression)Visit(context.expr()))
        {
            LineNumber = _lineNumber
        };
    }
    public override ASTNode VisitIfBlock(EduGrammarParser.IfBlockContext context)
    {
        var condition = (Expression)Visit(context.expr());
        var ifBlock = (BlockStatement)Visit(context.block());
        var elseBlock = context.elseBlock() != null ? (BlockStatement)Visit(context.elseBlock()) : null;

        return new IfBlock(condition, ifBlock, elseBlock)
        {
            LineNumber = _lineNumber
        };
    }
    public override ASTNode VisitElseBlock(EduGrammarParser.ElseBlockContext context)
    {
        return Visit(context.block());
    }
    public override ASTNode VisitWhileBlock(EduGrammarParser.WhileBlockContext context)
    {
        var condition = (Expression)Visit(context.expr());
        var block = (BlockStatement)Visit(context.block());

        return new WhileBlock(condition, block)
        {
            LineNumber = _lineNumber
        };
    }
    public override ASTNode VisitForLoop(EduGrammarParser.ForLoopContext context)
    {
        var init = (Statement)Visit(context.variableDeclaration());
        var condition = (Expression)Visit(context.expr());
        var update = (Statement)Visit(context.assignment());
        var block = (BlockStatement)Visit(context.block());

        return new ForLoopStatement(init, condition, update, block)
        {
            LineNumber = _lineNumber
        };
    }
    public override ASTNode VisitReturnStatement(EduGrammarParser.ReturnStatementContext context)
    {
        return new ReturnStatement((Expression)Visit(context.expr()))
        {
            LineNumber = _lineNumber
        };
    }
    public override ASTNode VisitFunctionCall(EduGrammarParser.FunctionCallContext context)
    {
        var functionName = context.id().GetText();
        var arguments = context.argumentList() != null ? VisitArgumentList(context.argumentList()) as ArgumentList : new ArgumentList();

        return new FunctionCallStatement(functionName, arguments)
        {
            LineNumber = _lineNumber
        };
    }
    public override ASTNode VisitArgumentList(EduGrammarParser.ArgumentListContext context)
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

        var argumentNode = new ArgumentList(arguments);
        return argumentNode;
    }
    // Continue with other methods for other types of nodes
}
