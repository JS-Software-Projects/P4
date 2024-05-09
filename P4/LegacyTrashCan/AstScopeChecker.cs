using System;
using System.Collections.Generic;
using P4.Interpreter.AST;
/*
namespace P4.Interpreter


    public class AstScopeChecker : IASTVisitor<ASTNode>
    {


        public AstScopeChecker()
        {
            // Initialize with a global scope
           // _scopes.Push(new Dictionary<string, TypeNode>());
        }




        public void Visit(ASTNode node)
        {
            node.Visit(this);
        }

        ,
        private void VisitBlock(BlockNode blockNode)
        {
            _scopes.Push(new Dictionary<string, TypeNode>()); // Enter new scope for the block
            foreach (var statement in blockNode.Statements)
            {
               // Visit(statement);
            }
            _scopes.Pop(); // Exit scope
        }

        public void VisitVariableDeclaration(VariableDeclarationNode varDeclNode)
        {
            var varName = varDeclNode.GetVariableName().ToString();
            if (IsVariableDeclared(varName))
            {
                throw new Exception($"Variable '{varName}' already declared.");
            }
            var varType = (TypeNode)varDeclNode.GetVariableType();
            _scopes.Peek().Add(varName, varType);
        }

        public void VisitAssignment(AssignmentNode assignNode)
        {
            var varName = assignNode.GetVariableName().ToString();
            if (!IsVariableDeclared(varName))
            {
                throw new Exception($"Variable '{varName}' not declared.");
            }
        }

        private bool IsVariableDeclared(string varName)
        {
            return _scopes.Peek().ContainsKey(varName);
        }



        public void PrintScopes()
        {
            foreach (var scope in _scopes)
            {
                Console.WriteLine("Scope:");
                foreach (var (varName, varType) in scope)
                {
                    Console.WriteLine($"  {varName}: {varType}");
                }
            }
        }

        public ASTNode Visit(AST.ASTNode node)
        {
            return null;
        }

        public ASTNode Visit(Block node)
        {
            throw new NotImplementedException();
        }

        public ASTNode Visit(BinaryExpression node)
        {
            throw new NotImplementedException();
        }

        public ASTNode Visit(UnaryExpression node)
        {
            throw new NotImplementedException();
        }

        public ASTNode Visit(TernaryExpression node)
        {
            throw new NotImplementedException();
        }

        public ASTNode Visit(Assignment node)
        {
            throw new NotImplementedException();
        }

        public ASTNode Visit(FunctionCall node)
        {
            throw new NotImplementedException();
        }

        public ASTNode Visit(VariableDeclaration node)
        {
            throw new NotImplementedException();
        }

        public ASTNode Visit(Expression node)
        {
            throw new NotImplementedException();
        }

        public ASTNode Visit(Statement node)
        {
            throw new NotImplementedException();
        }

        public ASTNode Visit(ConstantExpression node)
        {
            throw new NotImplementedException();
        }

        public ASTNode Visit(IdentifierExpression node)
        {
            throw new NotImplementedException();
        }

        public ASTNode Visit(ParameterNode node)
        {
            throw new NotImplementedException();
        }

        public ASTNode Visit(IfNode node)
        {
            throw new NotImplementedException();
        }

        public ASTNode Visit(WhileNode node)
        {
            throw new NotImplementedException();
        }

        public ASTNode Visit(ReturnNode node)
        {
            throw new NotImplementedException();
        }

        public ASTNode Visit(ForNode node)
        {
            throw new NotImplementedException();
        }

        public ASTNode Visit(Print node)
        {
            throw new NotImplementedException();
        }

        public ASTNode Visit(IfBlock node)
        {
            throw new NotImplementedException();
        }

        public ASTNode Visit(WhileBlock node)
        {
            throw new NotImplementedException();
        }

        public ASTNode Visit(ReturnStatement node)
        {
            throw new NotImplementedException();
        }

        public ASTNode Visit(ForLoop node)
        {
            throw new NotImplementedException();
        }
    }

}*/