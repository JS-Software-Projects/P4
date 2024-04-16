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
        var functionNode = new FunctionDeclarationNode {
            type = context.type().GetText(),
            Name = context.id().GetText(), // Correctly accessing the function's name
            Parameters = VisitParameterList(context.parameterList()),
            Body = VisitBlock(context.block()),
        };
        return functionNode;
    }

    public override ASTNode VisitVariableDeclaration(EduGrammarParser.VariableDeclarationContext context)
    {
        var type = context.type().GetText();
        var variableName = context.id().GetText();
        var expression = context.expr() != null ? Visit(context.expr()) : null;
        if (expression == null)
        {
            return VisitParameter(context.parameter());
        }
        
        return new VariableDeclarationNode {
            Type = type,
            VariableName = variableName,
            Expression = expression
        };
    }
    public override ASTNode VisitParameter(EduGrammarParser.ParameterContext context) {
        var typeName = context.type().GetText();
        var paramName = context.id().GetText();
        return new ParameterNode(paramName, typeName);
    }


    public override ASTNode VisitConstant(EduGrammarParser.ConstantContext context)
    {
        var constantNode = new ConstantNode(context.GetText());
        return constantNode;
    }

    public override ASTNode VisitBinaryExpr(EduGrammarParser.BinaryExprContext context)
    {
        var exprNode = new ExpressionNode
        {
            Operator = context.binOP().GetText(),
            Left = Visit(context.expr(0)),  // Visit the left expression once, use it for all cases.
            Right = Visit(context.expr(1))  // Visit the right expression once, use it for all cases.
        };
        return exprNode;
    }
    public override ASTNode VisitUnaryExpr(EduGrammarParser.UnaryExprContext context)
    {
        var exprNode = new ExpressionNode
        {
            Operator = context.unOP().GetText(),
            Right = Visit(context.expr())
        };
        return exprNode;
    }

    public override ASTNode VisitParenExpr(EduGrammarParser.ParenExprContext context)
    {   
        return Visit(context.expr());
    }

    public override ASTNode VisitIdentifier(EduGrammarParser.IdentifierContext context)
    {
       return new IdentifierNode(context.GetText());
    }

    public override ASTNode VisitTernaryExpr(EduGrammarParser.TernaryExprContext context) {
        var condition = Visit(context.expr(0));  // Visit the condition expression
        var trueExpr = Visit(context.expr(1));   // Visit the expression for the true branch
        var falseExpr = Visit(context.expr(2));  // Visit the expression for the false branch

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


    // Continue with other methods for other types of nodes
}
