using P4.Interpreter;
using P4.Interpreter.AST.Nodes;

namespace P4.Tests.Interpreter.ScopeTypeCheckerTests;

public class VisitFunctionCallShould
{
    private readonly ScopeTypeChecker _checker = new();
    
    
    [Fact]
    public void Visit_FunctionCallStatement_FunctionNotDeclared_ThrowsException()
    {
        var callfuncName = "testFunc";
        var argument1 =new ConstantExpression("String");
        var arguments = new List<Expression> { argument1 };
        var node = new FunctionCallStatement(callfuncName, arguments);
        node.LineNumber = 1;

        var exception = Assert.Throws<Exception>(() => _checker.Visit(node));
        Assert.Equal("Function not declared. In line: 1", exception.Message);
    }

    [Fact]
    public void Visit_FunctionCallStatement_TypeMismatch_ThrowsException()
    {
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
        
         _checker.Visit(funcDecl);
        var callFuncName = "testFunc";
        var argument1 =new ConstantExpression("String");
        var arguments = new List<Expression> { argument1 };
        var node = new FunctionCallStatement(callFuncName, arguments);
       
        var exception = Assert.Throws<Exception>(() => _checker.Visit(node));
        Assert.Equal("Type mismatch in function call does not match declaration of testFunc", exception.Message);
    }

    [Fact]
    public void Visit_FunctionCallStatement_CorrectFunctionCall_ReturnsNull()
    {
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
         _checker.Visit(funcDecl);
        var callfuncName = "testFunc";
        var argument1 =new ConstantExpression(1);
        var arguments = new List<Expression> { argument1 };
        var node = new FunctionCallStatement(callfuncName, arguments);
       
        var result = _checker.Visit(node);
        
        Assert.Null(result);
    }
}