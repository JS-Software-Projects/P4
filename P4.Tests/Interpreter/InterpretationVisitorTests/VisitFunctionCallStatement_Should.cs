using P4.Interpreter;
using P4.Interpreter.AST.Nodes;

namespace P4.Tests.Interpreter.InterpretationVisitorTests;

public class VisitFunctionCallStatement_Should
{
    private readonly InterpretationVisitor _visitor = new();
    
    [Fact]
    public void Visit_FunctionCallStatement_ReturnsCorrect()
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
        
        _visitor.Visit(funcDecl);
        var callFuncName = "testFunc";
        var argument1 =new ConstantExpression(12);
        var arguments = new List<Expression> { argument1 };
        var node = new FunctionCallStatement(callFuncName, arguments);

        var result = _visitor.Visit(node);
            
        Assert.Equal(1,result);
    }
}