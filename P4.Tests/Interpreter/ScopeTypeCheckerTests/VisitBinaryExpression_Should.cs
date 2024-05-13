using System;
using P4.Interpreter;
using Xunit;
using P4.Interpreter.AST;
using P4.Interpreter.AST.Nodes;

namespace P4.Tests.Interpreter.ScopeTypeCheckerTests;

    public class VisitBinaryExpressionShould
    {
        private readonly ScopeTypeChecker _checker = new();

        [Fact]
        public void Visit_BinaryExpression_Add_Numbers_ReturnsNumType()
        {
            var leftExpression = new ConstantExpression(1);
            var rightExpression = new ConstantExpression(2);
            
            var node = new BinaryExpression(leftExpression, Operator.Add, rightExpression);

            var result = _checker.Visit(node);

            Assert.Equal("Num", result.TypeName);
        }

        [Fact]
        public void Visit_BinaryExpression_Add_StringAndNumber_ThrowsException()
        {
            var leftExpression = new ConstantExpression("test");
            var rightExpression = new ConstantExpression(2);
            
            var node = new BinaryExpression(leftExpression, Operator.Add, rightExpression);


            Assert.Throws<Exception>(() => _checker.Visit(node));
        }

        [Fact]
        public void Visit_BinaryExpression_Add_Strings_ReturnsStringType()
        {
            var leftExpression = new ConstantExpression("test");
            var rightExpression = new ConstantExpression("test");

            var node = new BinaryExpression(leftExpression, Operator.Add, rightExpression);
            
            var result = _checker.Visit(node);

            Assert.Equal("String", result.TypeName);
        }
        
        [Fact]
        public void Visit_BinaryExpression_Subtract_NumberAndString_Throws()
        {
            var leftExpression = new ConstantExpression("test");
            var rightExpression = new ConstantExpression(2);

            var node = new BinaryExpression(leftExpression, Operator.Subtract, rightExpression);
            
            

            Assert.Throws<Exception>(() => _checker.Visit(node));
        }
        
        [Fact]
        public void Visit_BinaryExpression_Subtract_Numbers_ReturnsNumType()
        {
            var leftExpression = new ConstantExpression(1);
            var rightExpression = new ConstantExpression(2);

            var node = new BinaryExpression(leftExpression, Operator.Subtract, rightExpression);
            
            var result = _checker.Visit(node);

            Assert.Equal("Num", result.TypeName);
        }
        
        [Fact]
        public void Visit_BinaryExpression_LessThan_Numbers_ReturnsBoolType()
        {
            var leftExpression = new ConstantExpression(1);
            var rightExpression = new ConstantExpression(2);

            var node = new BinaryExpression(leftExpression, Operator.LessThan, rightExpression);
            
            var result = _checker.Visit(node);

            Assert.Equal("Bool", result.TypeName);
        }
        
        [Fact]
        public void Visit_BinaryExpression_LessThan_Strings_ThrowsException()
        {
            var leftExpression = new ConstantExpression("test");
            var rightExpression = new ConstantExpression("test");

            var node = new BinaryExpression(leftExpression, Operator.LessThan, rightExpression);
            
            Assert.Throws<Exception>(() => _checker.Visit(node));
        }
        
        [Fact]
        public void Visit_BinaryExpression_Equal_Numbers_ReturnsBoolType()
        {
            var leftExpression = new ConstantExpression(1);
            var rightExpression = new ConstantExpression(2);

            var node = new BinaryExpression(leftExpression, Operator.Equal, rightExpression);
            
            var result = _checker.Visit(node);

            Assert.Equal("Bool", result.TypeName);
        }
        
        [Fact]
        public void Visit_BinaryExpression_Equal_Bool_ReturnsBoolType()
        {
            var leftExpression = new ConstantExpression(false);
            var rightExpression = new ConstantExpression(false);

            var node = new BinaryExpression(leftExpression, Operator.Equal, rightExpression);
            
            var result = _checker.Visit(node);

            Assert.Equal("Bool", result.TypeName);
        }
        
        [Fact]
        public void Visit_BinaryExpression_Equal_BoolAndNumber_ThrowsException()
        {
            var leftExpression = new ConstantExpression(false);
            var rightExpression = new ConstantExpression(2);

            var node = new BinaryExpression(leftExpression, Operator.Equal, rightExpression);
            
            Assert.Throws<Exception>(() => _checker.Visit(node));
        }
        
        [Fact]
        public void Visit_BinaryExpression_UnknownOperator_ThrowsException()
        {
            var leftExpression = new ConstantExpression(2);
            var rightExpression = new ConstantExpression(2);

            var node = new BinaryExpression(leftExpression, Operator.Not , rightExpression);
            
            Assert.Throws<Exception>(() => _checker.Visit(node));
        }
        
        [Fact]
        public void Visit_BinaryExpression_Or_Bool_ReturnsBoolType()
        {
            var leftExpression = new ConstantExpression(false);
            var rightExpression = new ConstantExpression(false);

            var node = new BinaryExpression(leftExpression, Operator.Or, rightExpression);
            
            var result = _checker.Visit(node);

            Assert.Equal("Bool", result.TypeName);
        }
        [Fact]
        public void Visit_BinaryExpression_Or_Bool_ThrowsException()
        {
            var leftExpression = new ConstantExpression(2);
            var rightExpression = new ConstantExpression(2);

            var node = new BinaryExpression(leftExpression, Operator.Or, rightExpression);
            
            Assert.Throws<Exception>(() => _checker.Visit(node));
        }
    }
