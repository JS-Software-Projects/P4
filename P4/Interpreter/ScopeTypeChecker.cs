using System;
using System.Collections.Generic;
using P4.Interpreter.AST;

namespace P4.Interpreter;
using P4.Interpreter.AST.Nodes;

public class ScopeTypeChecker : IASTVisitor<Type>
{
    private readonly SymbolTable<string,Type> _symbolTableType = new();

    public Type Visit(ASTNode node)
    {
        return node.Accept(this);
    }
    public Type Visit(ProgramNode node)
    {
        foreach (var child in node.GetChildren())
        {
            Visit(child);
        }
        return null;
    }
    public Type Visit(BlockStatement node)
    {
        foreach (var statement in node.Statements)
        {
            if (statement is ReturnStatement)
            {
                return Visit(statement);
            }
            Visit(statement);
        }
        return null;
    }

    public Type Visit(BinaryExpression node)
    {
        Type leftType = Visit(node.Left);
        Type rightType = Visit(node.Right);
        
        if (leftType.TypeName != rightType.TypeName)
        {
            throw new Exception("Type mismatch in binary expression. In line:"+node.LineNumber);
        }
        
        switch (node.Operator)
        {
            case Operator.Add:
                if(leftType.TypeName == "Num" || leftType.TypeName == "String")
                {
                    return leftType;
                }
                else
                {
                    throw new Exception("Type mismatch in binary expression: expected a Number or a String. In line:"+node.LineNumber);
                }
            case Operator.Subtract:
            case Operator.Multiply:
            case Operator.Divide:
                if (leftType.TypeName == "Num")
                {
                    return leftType;
                }
                else
                {
                    throw new Exception("Type mismatch in binary expression: expected a Number. In line:"+node.LineNumber);
                }
            case Operator.LessThan:
            case Operator.LessThanOrEqual:
            case Operator.GreaterThan:
            case Operator.GreaterThanOrEqual:
                if (leftType.TypeName == "Num")
                {
                    return new Type("Bool");
                }
                else
                {
                    throw new Exception("Type mismatch in binary expression: expected a Number. In line:"+node.LineNumber);
                }
            case Operator.Equal:
            case Operator.NotEqual:
                if (leftType.TypeName == rightType.TypeName)
                {
                    return new Type("Bool");
                }
                else
                {
                    throw new Exception("Type mismatch in binary expression: expected a Number. In line:"+node.LineNumber);
                }
            case Operator.Or:
            case Operator.And:
                if (leftType.TypeName == "Bool")
                {
                    return new Type("Bool");
                }
                else
                {
                    throw new Exception("Type mismatch in binary expression: expected a Bool.");
                }
               
            default:
                throw new Exception("Unknown binary operator. In line:"+node.LineNumber);
        }
    }

    public Type Visit(UnaryExpression node)
    {
        Type operandType = Visit(node.Operand);
        
        switch (node.Operator)
        {
            case Operator.Not:
                if (operandType.TypeName != "Bool")
                {
                    throw new Exception("Type mismatch in unary expression: expected Bool. In line:"+node.LineNumber);
                }
                return operandType;
            case Operator.Subtract:
                if (operandType.TypeName != "Num")
                {
                    throw new Exception("Type mismatch in unary expression: expected a Number. In line:"+node.LineNumber);
                }
                return operandType;
            default:
                throw new Exception("Unknown unary operator. In line:"+node.LineNumber);
        }
    }

    public Type Visit(TernaryExpression node)
    {
        
        Type conditionType = Visit(node.Condition);
        if (conditionType.TypeName != "Bool")
        {
            throw new Exception("Condition in ternary expression must be of type Bool. In line:"+node.LineNumber);
        }
        
        Type trueExpressionType = Visit(node.ThenExpression);
        Type falseExpressionType = Visit(node.ElseExpression);

        if (trueExpressionType.TypeName != falseExpressionType.TypeName)
        {
            throw new Exception("True and false expressions in a ternary operation must be of the same type. In line:"+node.LineNumber);
        }
        
        return trueExpressionType;
    }

    
    public Type Visit(AssignmentStatement node)
    {
        // Scope rule
        var varName = node.VariableName;
        if (!_symbolTableType.IsVariableDeclared(varName.Name))
        {
            throw new Exception("Variable not declared. In line:"+node.LineNumber);
        }
        //Type checking
        Type expression = Visit(node.Expression);
        
        if (!_symbolTableType.IsTypeCorrect(varName.Name, expression))
        {
            throw new Exception("Type mismatch. In line:"+node.LineNumber);
        }
        return null;
    }

    public Type Visit(FunctionCallStatement node)
    {
        if (!_symbolTableType.IsVariableDeclared(node.FunctionName))
        {
            throw new Exception("Function not declared. In line: "+node.LineNumber);
        }

        var functionType = _symbolTableType.Get(node.FunctionName) as TypeExtended;
        for (int i = 0; i < node.Arguments.Arguments.Count; i++)
        {
            if (functionType != null && functionType.Args[i].TypeName != Visit(node.Arguments.Arguments[i]).TypeName)
            {
                throw new Exception("Type mismatch in function call does not match declaration of "+node.FunctionName);
            }
        }

        return null;
    }

    public Type Visit(GameObjectDeclaration node)
    {
        if (_symbolTableType.IsVariableDeclared(node.ObjectName.Name))
        {
            throw new Exception("gameObject already declared. In line:"+node.LineNumber);
        }
        
        if (!node.ObjectType.IsCorrectType())
        {
            throw new Exception("Unknown gameClass in GameObject declaration. In line:"+node.LineNumber);
        }
        var typeList = new List<Type>();

        List<Type> TowerTypes = new List<Type>();
        TowerTypes.Add(new Type("Num"));
        TowerTypes.Add(new Type("Num"));
        
        if (node.ObjectType.TypeName == "Tower")
        {
            if (node.ArgumentLists.Arguments.Count != TowerTypes.Count)
            {
                throw new Exception("Incorrect number of arguments in Tower GameObject declaration. Needs two arguments (Num,Num) In line:"+node.LineNumber);
            }
            for (int i = 0; i < node.ArgumentLists.Arguments.Count; i++)
            {
                if (node.ArgumentLists.Arguments[i] != null){
                    if (Visit(node.ArgumentLists.Arguments[i]).TypeName != TowerTypes[i].TypeName)
                    {
                        throw new Exception("Type mismatch in Tower GameObject declaration. In line:"+node.LineNumber);
                    }
                }
            }
            
        }

     
        var type = new Type(node.ObjectType.TypeName);
        
        
        _symbolTableType.Add(node.ObjectName.Name, type);
        return null;
    }

    public Type Visit(GameObjectMethodCall node)
    {
        if (!_symbolTableType.IsVariableDeclared(node.ObjectName.Name))
        {
            throw new Exception("GameObject not declared. In line:"+node.LineNumber);
        }
        var objType = _symbolTableType.Get(node.ObjectName.Name);
        if (node.MethodName == "move" && objType.TypeName == "Hero")
        {
            if (node.ArgumentList.Arguments.Count != 2)
            {
                throw new Exception("Incorrect number of arguments in Hero GameObject call. Needs one argument (Num) In line:"+node.LineNumber);
            }
            if (Visit(node.ArgumentList.Arguments[0]).TypeName != "Num" && Visit(node.ArgumentList.Arguments[1]).TypeName != "Num")
            {
                throw new Exception("Type mismatch in Hero GameObject call. In line:"+node.LineNumber);
            }
        }

        return null;
    }


    public Type Visit(VariableDeclaration node)
    {
        // Scope rule 
        var varName = node.VariableName;
        if (_symbolTableType.IsVariableDeclaredInScope(varName.Name))
        {
            throw new Exception($"Variable '{varName}' already declared. In line:"+node.LineNumber);
        }

        // Type checking
        var varType = new Type(node.Type.TypeName);
        if (!varType.IsCorrectType())
        {
            throw new Exception("Unknown type in variable declaration. In line:"+node.LineNumber);
        }
        
        var expressionType = Visit(node.Expression);
        if (varType.TypeName != expressionType.TypeName)
        {
            throw new Exception($"Type mismatch for variable '{varName}'. In line:"+node.LineNumber);
        }
        
        _symbolTableType.Add(varName.Name, varType);
        return null;
    }
    


    public Type Visit(ConstantExpression node)
    {
        if (node.Value is int || node.Value is double)
        {
            return new Type("Num");
        }
        else if (node.Value is bool)
        {
            return new Type("Bool");
        }
        else if (node.Value is string)
        {
            return new Type("String");
        }
        else
        {
            throw new Exception("Unknown type in constant expression. In line:"+node.LineNumber);
        }
    }
    public Type Visit(IdentifierExpression node)
    {
        var varName = node.Name;
        if (!_symbolTableType.IsVariableDeclared(varName))
        {
            throw new Exception($"Variable '{varName}' not declared. In line:"+node.LineNumber);
        }
        return _symbolTableType.Get(varName) as Type;
    }

    public Type Visit(ParameterNode node)
    {
        if (_symbolTableType.IsVariableDeclaredInScope(node.ParameterName.Name))
        {
            throw new Exception("Parameter already declared. In line:"+node.LineNumber);
        }
        var parType = new Type(node.Type.TypeName);
        if (!parType.IsCorrectType())
        {
            throw new Exception("Unknown type in parameter declaration. In line:"+node.LineNumber);
        }

        return parType;
    }

    public Type Visit(FunctionDeclaration node)
    {
        if (_symbolTableType.IsVariableDeclared(node.FunctionName.Name))
        {
            throw new Exception("Function already declared. In line:"+node.LineNumber);
        }
        
        var typeList = new List<Type>();

        foreach (var parameter in node.ParameterList.Parameters)
        {
            typeList.Add(Visit(parameter));
        }
        
        var returnType = new TypeExtended(typeList,node.ReturnType.TypeName);
        if (!returnType.IsCorrectTypeE())
        {
            throw new Exception("Unknown type in function declaration. In line:"+node.LineNumber);
        }
        
        _symbolTableType.PushScope();
        foreach (var parameter in node.ParameterList.Parameters)
        {
            _symbolTableType.Add(parameter.ParameterName.Name, Visit(parameter));
        }
        var blockType = Visit(node.Block);
        _symbolTableType.PopScope();
        if (returnType.TypeName == "Void" && blockType == null)
        {
            _symbolTableType.Add(node.FunctionName.Name, returnType);
            return null;
        }

        if (returnType.TypeName != "void" && blockType == null)
        {
            throw new Exception("Function must return a value. In line:"+node.LineNumber);
        }

        if (returnType.TypeName != blockType.TypeName )
        {
            throw new Exception("Return type mismatch. In line:"+node.LineNumber);
        }
        _symbolTableType.Add(node.FunctionName.Name, returnType);
        return null;
    }

    public Type Visit(PrintStatement node)
    {
        Visit(node.Expression);
        return null;
    }

    public Type Visit(IfBlock node)
    {
        if (Visit(node.Condition).TypeName != "Bool")
        {
            throw new Exception("Condition in while block must be of type Bool. In line:"+node.LineNumber);
        }
        _symbolTableType.PushScope(); // Enter new scope for the if block
        Visit(node.Block); // Visit children
        _symbolTableType.PopScope(); // Exit scope
        return null;
    }

    public Type Visit(WhileBlock node)
    {
        if (Visit(node.Condition).TypeName != "Bool")
        {
            throw new Exception("Condition in while block must be of type Bool. In line:"+node.LineNumber);
        }
        _symbolTableType.PushScope(); // Enter new scope for the while block
        Visit(node.Block); // Visit children
        _symbolTableType.PopScope(); // Exit scope
        return null;
    }

    public Type Visit(ReturnStatement node)
    {
        return Visit(node.Expression);
    }

    public Type Visit(ForLoopStatement node)
    {   
        Visit(node.Initialization);
        if (Visit(node.Condition).TypeName != "Bool")
        {
            throw new Exception("Condition in for loop must be of type Bool. In line:"+node.LineNumber);
        }
        _symbolTableType.PushScope(); // Enter new scope for the for loop
        Visit(node.Block);
        Visit(node.Increment);
        _symbolTableType.PopScope(); // Exit scope
        return null;
    }
}