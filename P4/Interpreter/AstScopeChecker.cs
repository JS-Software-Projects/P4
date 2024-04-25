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

        public void Visit(ASTNode node)
        {
            switch (node)
            {
                case BlockNode blockNode:
                    VisitBlock(blockNode);
                    break;
                case VariableDeclarationNode varDeclNode:
                    VisitVariableDeclaration(varDeclNode);
                    break;
                case AssignmentNode assignNode:
                    VisitAssignment(assignNode);
                    break;
                // Add more cases as needed for other ASTNode types
                default:
                    throw new Exception($"Unsupported node type: {node.GetType()}");
            }
        }

        private void VisitBlock(BlockNode blockNode)
        {
            _scopes.Push(new Dictionary<string, TypeNode>()); // Enter new scope for the block
            foreach (var statement in blockNode.Statements)
            {
                Visit(statement);
            }
            _scopes.Pop(); // Exit scope
        }

        private void VisitVariableDeclaration(VariableDeclarationNode varDeclNode)
        {
            var varName = varDeclNode.GetVariableName().ToString();
            if (IsVariableDeclared(varName))
            {
                throw new Exception($"Variable '{varName}' already declared.");
            }
            var varType = (TypeNode)varDeclNode.GetVariableType();
            _scopes.Peek().Add(varName, varType);
        }

        private void VisitAssignment(AssignmentNode assignNode)
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
    }
}