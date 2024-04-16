using System.Collections.Generic;

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
            Name = context.id().GetText(), // Correctly accessing the function's name
            Parameters = VisitParameterList(context.parameterList())
        };

        // Initialize the body of the function as a new node that can contain multiple statements
        var bodyNode = new CompoundStatementNode();
        foreach (var lineContext in context.line()) {
            var lineNode = Visit(lineContext); // Visit each line and add it to the body
            if (lineNode != null) {
                bodyNode.Children.Add(lineNode);
            }
        }
        functionNode.Body = bodyNode;

        return functionNode;
    }

    public override ASTNode VisitVariableDeclaration(EduGrammarParser.VariableDeclarationContext context)
    {
        if (context.expr() == null) {
            // If there is no expression, create a VariableDeclarationNode with null expression
            return new VariableDeclarationNode {
                VariableName = context.id().GetText(),
                Expression = null
            };
        }
        var variableNode = new VariableDeclarationNode {
            VariableName = context.id().GetText(),
            Expression = Visit(context.expr())
        };
        return variableNode;
    }

    public override ASTNode VisitConstant(EduGrammarParser.ConstantContext context)
    {
        var constantNode = new ConstantNode(context.GetText());
        return constantNode;
    }

    public override ASTNode VisitBinaryExpr(EduGrammarParser.BinaryExprContext context)
    {
        var exprNode = new ExpressionNode {
            Operator = context.binOP().GetText(),
            Left = Visit(context.expr(0)),
            Right = Visit(context.expr(1))
        };
        return exprNode;
    }
    // Continue with other methods for other types of nodes
}
