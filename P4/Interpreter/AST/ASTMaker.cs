﻿using System;
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
        foreach (var child in context.children)
        {
            var result = Visit(child);
            if (result != null)
            {
                programNode.AddChild(result);
            }
        }
        return programNode;
    }
    public override ASTNode VisitLine(EduGrammarParser.LineContext context)
    {
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
        var type = VisitType(context.type()) as Type;
        if (type == null)
        {
            throw new InvalidCastException("Expected Type");
        }
        var typeName = type.TypeName;
        var name = context.id().GetText(); // Correctly accessing the function's name
        var parameters = VisitParameterList(context.parameterList());
        var body = VisitBlock(context.block());

        return new FunctionDeclaration(typeName, name, parameters, body);
    }

    public override ASTNode VisitVariableDeclaration(EduGrammarParser.VariableDeclarationContext context)
    {
        var type = context.type().GetText();
        var variableName = context.id().GetText();
        var expression = context.expr() != null ? Visit(context.expr()) as Expression : null;
        
        return new VariableDeclaration(variableName,type, expression);
    }

    public override ASTNode VisitConstant(EduGrammarParser.ConstantContext context)
    {
        return new ConstantExpression(context.GetText());
    }

    public override ASTNode VisitComparisonExpr(EduGrammarParser.ComparisonExprContext context)
    {
        var op = context.compareOp();
        
        if (op != null)
        {
            var left = Visit(context.additionExpr(0)) as Expression;
            var right = Visit(context.additionExpr(1)) as Expression;
            
            return new BinaryExpression(left,Operator.Or, right);
        }

        return VisitAdditionExpr(context.additionExpr(0));
    }
    
    public override ASTNode VisitMultiplicationExpr(EduGrammarParser.MultiplicationExprContext context)
    {
        if (context.MULT() == null | context.DIV() == null)
        {
            return Visit(context.unaryExpr(0));
        }
        
        var oOperator = Operator.Add;
        if (context.MULT()!= null )
        {
            
            oOperator = Operator.Multiply;
        }
        if (context.DIV() != null)
        {
            oOperator = Operator.Divide;
        }
        
        var left = Visit(context.unaryExpr(0)) as Expression;
        var right = Visit(context.unaryExpr(1)) as Expression;
            
        return new BinaryExpression(left, oOperator, right);
    }

    public override ASTNode VisitBoolExpr(EduGrammarParser.BoolExprContext context)
    {
        var op = context.boolOp();
        if (op != null)
        {
            var left = Visit(context.comparisonExpr(0)) as Expression;
            var right = Visit(context.comparisonExpr(1)) as Expression;
            return new BinaryExpression(left, Operator.Or, right);
        }
        return Visit(context.comparisonExpr(0));
    }
    
    public override ASTNode VisitAdditionExpr(EduGrammarParser.AdditionExprContext context)
    {
        var op = context.addSubOp();

        if (op != null)
        {
            var oOperator = Operator.Add;
            if (op.ToString() == "+")
            {
                oOperator = Operator.Add;
            } else if (op.ToString() == "-")
            {
                oOperator = Operator.Subtract;
            }
            var left = Visit(context.multiplicationExpr(0)) as Expression;
            var right = Visit(context.multiplicationExpr(1)) as Expression;
            
            return new BinaryExpression(left, oOperator, right);
        }
        else
        {
            return Visit(context.multiplicationExpr(0));
        }
    }

    public override ASTNode VisitTernaryExpr(EduGrammarParser.TernaryExprContext context)
    {
        if (context.term(1) != null)
        {
            var condition = Visit(context.term(0)) as Expression;
            var trueExpr = Visit(context.term(1)) as Expression;
            var falseExpr = Visit(context.term(2)) as Expression;
            
            return new TernaryExpression(condition, trueExpr, falseExpr);
        }
        return Visit(context.term(0));
    }
    
    
    public override ASTNode VisitUnaryExpr(EduGrammarParser.UnaryExprContext context)
    {
        if (context.unOP() == null)
        {
            return Visit(context.ternaryExpr());
        }
        
        var op = context.unOP();
        var right = Visit(context.ternaryExpr()) as Expression;
        
        return new UnaryExpression(Operator.Or, right);
    }

    public override ASTNode VisitParenExpr(EduGrammarParser.ParenExprContext context)
    {   
        return Visit(context.expr());
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
