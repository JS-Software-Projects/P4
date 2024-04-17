using System;
using System.Collections.Generic;
using Antlr4.Runtime.Atn;

namespace P4.Interpreter.AST;
using P4.Interpreter;

public class ASTMaker : EduGrammarBaseVisitor<ASTNode>
{
    public override ASTNode VisitProgram(EduGrammarParser.ProgramContext context)
    {
        var programNode = new ProgramNode();
            foreach (var child in context.children) {
                var result = Visit(child);
                if (result != null) {
                    programNode.AddChild(result);
                }
            }
        return programNode;
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
        var type = context.type().GetText();
        var name = context.id().GetText(); // Correctly accessing the function's name
        var parameters = VisitParameterList(context.parameterList());
        var body = VisitBlock(context.block());
        
         return new FunctionDeclarationNode(type,name,parameters,body);
    }

    public override ASTNode VisitVariableDeclaration(EduGrammarParser.VariableDeclarationContext context)
    {
        var type = context.type().GetText();
        var variableName = context.id().GetText();
        var expression = context.expr() != null ? Visit(context.expr()) : null;
        
        return new VariableDeclarationNode(type, variableName, expression);
    }

    public override ASTNode VisitConstant(EduGrammarParser.ConstantContext context)
    {
        return new ConstantNode(context.GetText());
    }

    public override ASTNode VisitBinaryExpr(EduGrammarParser.BinaryExprContext context)
    {
        var op = context.binOP().GetText();
        var left = Visit(context.expr(0));  // Visit the left expression once, use it for all cases.
        var right = Visit(context.expr(1));  // Visit the right expression once, use it for all cases.
       
        return new ExpressionNode(op, left, right);
    }
    public override ASTNode VisitUnaryExpr(EduGrammarParser.UnaryExprContext context)
    {

        var op = context.unOP().GetText();
        var right = Visit(context.expr());
        
        return new ExpressionNode(op, null, right);
    }

    public override ASTNode VisitParenExpr(EduGrammarParser.ParenExprContext context)
    {   
        return Visit(context.expr());
    }

    public override ASTNode VisitIdentifier(EduGrammarParser.IdentifierContext context)
    {
       return new IdentifierNode(context.GetText());
    }
    public override ASTNode VisitId(EduGrammarParser.IdContext context)
    {
        return new IdentifierNode(context.GetText());
    }
    
    public override ASTNode VisitTernaryExpr(EduGrammarParser.TernaryExprContext context)
    {
        var condition = Visit(context.expr(0)); // Visit the condition expression
        var trueExpr = Visit(context.expr(1)); // Visit the expression for the true branch
        var falseExpr = Visit(context.expr(2)); // Visit the expression for the false branch
        
        return new TernaryExpressionNode(condition, trueExpr, falseExpr);
    }
    public override ASTNode VisitBlock(EduGrammarParser.BlockContext context) {
        var block = new BlockNode();
        foreach (var line in context.line()) {
            var statementNode = Visit(line);
            if (statementNode != null) {
                block.Statements.Add(statementNode);
            } else {
                // Log or handle the case where Visit(line) returns null
            }
        }
        return block;
    }
    public override ASTNode VisitPrint(EduGrammarParser.PrintContext context)
    {
        return new PrintNode(Visit(context.expr()));
    }
    public override ASTNode VisitAssignment(EduGrammarParser.AssignmentContext context)
    {
        return new AssignmentNode(Visit(context.id()), Visit(context.expr()));
    }
    public override ASTNode VisitIfBlock(EduGrammarParser.IfBlockContext context)
    {
        var condition = Visit(context.expr());
        var ifBlock = Visit(context.block());
        var elseBlock = context.elseBlock() != null ? Visit(context.elseBlock()) : null;

        return new IfNode(condition, ifBlock, elseBlock);
    }
    public override ASTNode VisitElseBlock(EduGrammarParser.ElseBlockContext context)
    {
        return Visit(context.block());
    }
    public override ASTNode VisitWhileBlock(EduGrammarParser.WhileBlockContext context)
    {
        var condition = Visit(context.expr());
        var block = Visit(context.block());

        return new WhileNode(condition, block);
    }
    public override ASTNode VisitForLoop(EduGrammarParser.ForLoopContext context)
    {
        var init = Visit(context.variableDeclaration());
        var condition = Visit(context.expr());
        var update = Visit(context.assignment());
        var block = Visit(context.block());

        return new ForNode(init, condition, update, block);
    }
    public override ASTNode VisitReturnStatement(EduGrammarParser.ReturnStatementContext context)
    {
        return new ReturnNode(Visit(context.expr()));
    }
    public override ASTNode VisitFunctionCall(EduGrammarParser.FunctionCallContext context)
    {
        var functionName = context.id().GetText();
        var arguments = context.argumentList() != null ? VisitArgumentList(context.argumentList()) : new List<ASTNode>();

        return new FunctionCallNode(functionName, arguments);
    }
    public List<ASTNode> VisitArgumentList(EduGrammarParser.ArgumentListContext context)
    {
        var arguments = new List<ASTNode>();

        foreach (var exprCtx in context.expr())
        {
            arguments.Add(Visit(exprCtx));
        }

        return arguments;
    }
    // Continue with other methods for other types of nodes
}
