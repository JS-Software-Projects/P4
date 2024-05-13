using System;
using P4.Interpreter.AST;
using P4.Interpreter.AST.Nodes;

namespace P4.Interpreter;

public class InterpretationVisistor : IASTVisitor<object>
{
    private Environment _environment = new();
    public object Visit(ASTNode node)
    {
        return node.Accept(this);
    }

    public object Visit(ProgramNode node)
    {
        foreach (var child in node.GetChildren())
        {
            Visit(child);
        }
        return null;
    }

    public object Visit(BlockStatement node)
    {
        throw new System.NotImplementedException();
    }

    public object Visit(BinaryExpression node)
    {
        throw new System.NotImplementedException();
    }

    public object Visit(UnaryExpression node)
    {
        throw new System.NotImplementedException();
    }

    public object Visit(TernaryExpression node)
    {
        throw new System.NotImplementedException();
    }

    public object Visit(AssignmentStatement node)
    {
        throw new System.NotImplementedException();
    }

    public object Visit(FunctionCallStatement node)
    {
        var function = (FunctionDeclaration)_environment.Get(node.FunctionName);
        var originalEnvironment = _environment; // Save the current environment
        _environment = function.GetEnvironment().Copy(); // Switch to the function's environment
        _environment.PushScope();

        for (int i = 0; i < node.Arguments.Count; i++)
        {
            var param = function.ParameterList.Parameters[i];
            var argValue = Visit(node.Arguments[i]);
            _environment.DeclareVariable(param.ParameterName.Name);
            _environment.Add(param.ParameterName.Name, argValue);
        }

        var result = Visit(function.Block); // Execute the function body in the new environment

        _environment.PopScope();
        _environment = originalEnvironment; // Restore the original environment
        Console.WriteLine("should return result succes");
        return result;
    }

    public object Visit(VariableDeclaration node)
    {
        _environment.DeclareVariable(node.VariableName.Name);
        var value = Visit(node.Expression);
        _environment.Add(node.VariableName.Name, value);
        return null;
    }

    public object Visit(ConstantExpression node)
    {
        return node.Value;
    }

    public object Visit(IdentifierExpression node)
    {
        return node.Name;
    }

    public object Visit(ParameterNode node)
    {
        throw new System.NotImplementedException();
    }

    public object Visit(FunctionDeclaration node)
    {
        node.SetEnvironment(_environment);
        _environment.DeclareVariable(node.FunctionName.Name);
        _environment.Add(node.FunctionName.Name, node);
        return null;
    }

    public object Visit(PrintStatement node)
    {
        throw new System.NotImplementedException();
    }

    public object Visit(IfBlock node)
    {
        throw new System.NotImplementedException();
    }

    public object Visit(WhileBlock node)
    {
        throw new System.NotImplementedException();
    }

    public object Visit(ReturnStatement node)
    {
        throw new System.NotImplementedException();
    }

    public object Visit(ForLoopStatement node)
    {
        throw new System.NotImplementedException();
    }
}