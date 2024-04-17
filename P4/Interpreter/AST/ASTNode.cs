using System.Collections.Generic;
using System.Linq;

namespace P4.Interpreter.AST;

// Base class for all AST nodes
public abstract class ASTNode {
    protected List<ASTNode> Children { get; set; } = new();
}

public class BlockNode : ASTNode {
    public List<ASTNode> Statements { get; } = new();
    
    public override string ToString() {
        return "{" + string.Join(" ", Statements.Where(s => s != null).Select(s => s.ToString())) + "}";
    }

}

public class FunctionDeclarationNode : ASTNode {
    private string type { get; set; }
    private string Name { get; set; }
    private List<ParameterNode> Parameters { get; set; }
    private ASTNode Body { get; set; }
    public FunctionDeclarationNode(string type, string name, List<ParameterNode> parameters, ASTNode body)
    {
        this.type = type;
        Name = name;
        Parameters = parameters;
        Body = body;
    }
}

public class VariableDeclarationNode : ASTNode {
    private string Type { get; set; }
    private string VariableName { get; set; }
    private ASTNode Expression { get; set; }  // Can be null if no initial value is provided

    public VariableDeclarationNode(string type, string variableName, ASTNode expression)
    {
        Type = type;
        VariableName = variableName;
        Expression = expression;
    }
    public override string ToString() {
        return Expression != null
            ? $"Variable Declaration: {VariableName} = {Expression}"
            : $"Variable Declaration: {VariableName}";
    }
}

public class ExpressionNode : ASTNode {
    private string Operator { get; set; }
    private ASTNode Left { get; set; }
    private ASTNode Right { get; set; }
    
    public ExpressionNode(string op, ASTNode left, ASTNode right) {
        Operator = op;
        Left = left;
        Right = right;
    }
    public override string ToString() {
        var leftExpr = Left != null ? Left.ToString() : "null";
        var rightExpr = Right != null ? Right.ToString() : "null";
        return $"({leftExpr} {Operator} {rightExpr})";
    }

}
public class TernaryExpressionNode : ASTNode {
    private ASTNode Condition { get; set; }
    private ASTNode TrueExpr { get; set; }
    private ASTNode FalseExpr { get; set; }

    public TernaryExpressionNode(ASTNode condition, ASTNode trueExpr, ASTNode falseExpr) {
        Condition = condition;
        TrueExpr = trueExpr;
        FalseExpr = falseExpr;
    }
    public override string ToString() {
        return $"({Condition} ? {TrueExpr} : {FalseExpr})";
    }
}

public class ConstantNode : ASTNode {
    private object Value { get; set; }

    public ConstantNode(object value) {
        Value = value;
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}

public class IdentifierNode : ASTNode {
    private string Identifier { get; set; }

    public IdentifierNode(string identifier) {
        Identifier = identifier;
    }

    public override string ToString()
    {
        return Identifier;
    }
}

public class PrintNode : ASTNode {
    private ASTNode Expression { get; set; }
    public PrintNode(ASTNode expression)
    {
        Expression = expression;
    }
    public override string ToString()
    {
        return $"print {Expression}";
    }
}

public class AssignmentNode : ASTNode
{
    private ASTNode VariableName { get; set; }

    private ASTNode Expression { get; set; }

    private const string AssignmentOp = "=";

    public AssignmentNode(ASTNode variableName, ASTNode expression)
    {
        VariableName = variableName;
        Expression = expression;
    }
    public override string ToString()
    {
        return $"Assignment is: {VariableName} {AssignmentOp} {Expression}";
    }
}
public class IfNode : ASTNode
{
    private ASTNode Condition { get; set; }
    private ASTNode Body { get; set; }
    private ASTNode ElseBody { get; set; }
    public IfNode(ASTNode condition, ASTNode body, ASTNode elseBody)
    {
        Condition = condition;
        Body = body;
        ElseBody = elseBody;
    }

    public override string ToString()
    {
        return $"if {Condition} {Body} else {ElseBody}";
    }
}
public class WhileNode : ASTNode
{
    private ASTNode Condition { get; set; }
    private ASTNode Body { get; set; }

    public WhileNode(ASTNode condition, ASTNode body)
    {
        Condition = condition;
        Body = body;
    }
    public override string ToString()
    {
        return $"while {Condition} {Body}";
    }
}
public class ForNode : ASTNode
{
    private ASTNode Initializer { get; set; }
    private ASTNode Condition { get; set; }
    private ASTNode Increment { get; set; }
    private ASTNode Body { get; set; }

    public ForNode(ASTNode initializer, ASTNode condition, ASTNode increment, ASTNode body)
    {
        Initializer = initializer;
        Condition = condition;
        Increment = increment;
        Body = body;
    }
    public override string ToString()
    {
        return $"for {Initializer} {Condition} {Increment} {Body}";
    }
}
public class ReturnNode : ASTNode
{
    private ASTNode Expression { get; set; }
    public ReturnNode(ASTNode expression)
    {
        Expression = expression;
    }
    public override string ToString()
    {
        return $"return {Expression}";
    }
}
public class FunctionCallNode : ASTNode
{
    private string FunctionName { get; set; }
    private List<ASTNode> Arguments { get; set; }
    public FunctionCallNode(string functionName, List<ASTNode> arguments)
    {
        FunctionName = functionName;
        Arguments = arguments;
    }
    public override string ToString()
    {
        return $"{FunctionName}({string.Join(", ", Arguments)})";
    }
}