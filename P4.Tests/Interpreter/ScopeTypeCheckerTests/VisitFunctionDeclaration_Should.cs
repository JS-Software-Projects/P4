using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using P4.Interpreter;
using P4.Interpreter.AST;
using Xunit;
using P4.Interpreter.AST.Nodes;
namespace P4.Tests.Interpreter.ScopeTypeCheckerTests;

public class VisitFunctionDeclaration_Should
{
    private readonly ScopeTypeChecker _checker = new();

    [Fact]
    public void Visit_FunctionDeclaration_ReturnsNullWhenSuccessful()
    {
        // Arrange

        var funcName = new IdentifierExpression("testFunc");
        var funcType = new Type("Void");
        var funcBody = new BlockStatement();
        
        var paramName = new IdentifierExpression("testParam");
        var paramType = new Type("Num");
        
        var parameter1 = new ParameterNode(paramName, paramType);
        var parameterList = new List<ParameterNode>();
        parameterList.Add(parameter1);

        var parameters = new ParameterList(parameterList);

        var funcDecl = new FunctionDeclaration(funcType, funcName, parameters, funcBody);

        // act
        var result = _checker.Visit(funcDecl);

        // assert
        Assert.Equal(null, result);
    }
    [Fact]
    public void Visit_FunctionDeclaration_ReturnsNum_Successful()
    {
        // Arrange

        var funcName = new IdentifierExpression("testFunc");
        var funcType = new Type("Num");
        var funcBody = new BlockStatement();
        funcBody.Statements.Add(new ReturnStatement(new ConstantExpression(1)));
        
        var paramName = new IdentifierExpression("testParam");
        var paramType = new Type("Num");
        
        var parameter1 = new ParameterNode(paramName, paramType);
        var parameterList = new List<ParameterNode>();
        parameterList.Add(parameter1);

        var parameters = new ParameterList(parameterList);

        var funcDecl = new FunctionDeclaration(funcType, funcName, parameters, funcBody);

        // act
        var result = _checker.Visit(funcDecl);

        // assert
        Assert.Equal(null, result);
    }
    
    [Fact]
    public void Visit_FunctionDeclaration_IsDeclared_Throws()
    {
        // Arrange

        var funcName = new IdentifierExpression("testFunc");
        var funcType = new Type("Void");
        var funcBody = new BlockStatement();
       
        var paramName = new IdentifierExpression("testParam");
        var paramType = new Type("Num");
        
        var parameter1 = new ParameterNode(paramName, paramType);
        var parameterList = new List<ParameterNode>();
        parameterList.Add(parameter1);
        
        var parameters = new ParameterList(parameterList);

        var funcDecl = new FunctionDeclaration(funcType, funcName, parameters, funcBody);
        _checker.Visit(funcDecl);

        // act
        var funcDecl2 = new FunctionDeclaration(funcType, funcName, parameters, funcBody);

        // assert
        Assert.Throws<Exception>(() => _checker.Visit(funcDecl2));
        
    }
    [Fact]
    public void Visit_FunctionDeclaration_UnknownType_Throws()
    {
        // Arrange

        var funcName = new IdentifierExpression("testFunc");
        var funcType = new Type("unknownType");
        var funcBody = new BlockStatement();
       
        var paramName = new IdentifierExpression("testParam");
        var paramType = new Type("Num");
        
        var parameter1 = new ParameterNode(paramName, paramType);
        var parameterList = new List<ParameterNode>();
        parameterList.Add(parameter1);
        
        var parameters = new ParameterList(parameterList);
        

        // act
        var funcDecl = new FunctionDeclaration(funcType, funcName, parameters, funcBody);

        // assert
        Assert.Throws<Exception>(() => _checker.Visit(funcDecl));
        
    }
    [Fact]
    public void Visit_FunctionDeclaration_MissingReturnStatement_Throws()
    {
        // Arrange

        var funcName = new IdentifierExpression("testFunc");
        var funcType = new Type("Num"); // forventer et return statement med typen Num
        var funcBody = new BlockStatement();
       
        var paramName = new IdentifierExpression("testParam");
        var paramType = new Type("Num");
        
        var parameter1 = new ParameterNode(paramName, paramType);
        var parameterList = new List<ParameterNode>();
        parameterList.Add(parameter1);
        
        var parameters = new ParameterList(parameterList);
        

        // act
        var funcDecl = new FunctionDeclaration(funcType, funcName, parameters, funcBody);

        // assert
        Assert.Throws<Exception>(() => _checker.Visit(funcDecl));
        
    }
    [Fact]
    public void Visit_FunctionDeclaration_TypeMissmatch_Throws()
    {
        // Arrange

        var funcName = new IdentifierExpression("testFunc");
        var funcType = new Type("Num"); // forventer et Num return statement
        var funcBody = new BlockStatement();
        funcBody.Statements.Add(new ReturnStatement(new ConstantExpression("string")));
       
        var paramName = new IdentifierExpression("testParam");
        var paramType = new Type("Num");
        
        var parameter1 = new ParameterNode(paramName, paramType);
        var parameterList = new List<ParameterNode>();
        parameterList.Add(parameter1);
        
        var parameters = new ParameterList(parameterList);
        

        // act
        var funcDecl = new FunctionDeclaration(funcType, funcName, parameters, funcBody);

        // assert
        Assert.Throws<Exception>(() => _checker.Visit(funcDecl));
        
    }
    
    
}