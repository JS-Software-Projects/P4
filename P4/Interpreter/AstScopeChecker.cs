using System;
using System.Collections.Generic;
using P4.Interpreter.AST;

namespace P4.Interpreter
{
    public class AstScopeChecker
    {
        private readonly Stack<Dictionary<string, TypeNode>> _scopes = new();

        public AstScopeChecker()
        {
            // Initialize with a global scope
            _scopes.Push(new Dictionary<string, TypeNode>());
        }

        /*


        public void Visit(ASTNode node)
        {
            node.Visit(this);
        }

        */
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
        
    }
}