using System;
using System.Collections.Generic;
namespace Interpreter
{
    public class Parser
    {
        private readonly List<Token> tokens;
        private int current = 0;

        public Parser(List<Token> tokens)
        {
            this.tokens = tokens;
        }

        public List<Stmt> Parse()
        {
            List<Stmt> statements = new List<Stmt>();
            while (!IsAtEnd())
            {
                Stmt stmt = Declaration();
                if (stmt != null)
                {
                    statements.Add(stmt);
                }
            }

            return statements;
        }

        private Stmt Declaration()
        {
            try
            {
                if (Match(TokenType.Var)) return VarDeclaration();
                return Statement();
            }
            catch (ParseError)
            {
                Synchronize();
                return null;
            }
        }
        private Stmt VarDeclaration()
        {
            Token name = Consume(TokenType.Identifier, "Expect variable name.");

            Expr initializer = null;
            if (Match(TokenType.Equal))
            {
                initializer = Expression();
            }

            Consume(TokenType.Semicolon, "Expect ';' after variable declaration.");
            return new Var(name, initializer);
        }

        private Stmt Statement()
        {
            if (Match(TokenType.Print)) return PrintStatement();
            if (Match(TokenType.Left_Brace)) return new Block(Block());
            if (Match(TokenType.While)) return WhileStatement();
            return ExpressionStatement();
        }
        private Stmt PrintStatement()
        {
            Expr value = Expression();
            Consume(TokenType.Semicolon, "Expect ';' after value.");
            return new Print(value);
        }
        private Stmt ExpressionStatement()
        {
            Expr expr = Expression();
            Consume(TokenType.Semicolon, "Expect ';' after expression.");
            return new Expression(expr);
        }

        private Stmt WhileStatement()
        {
            Consume(TokenType.Left_Paren, "Expect '(' after 'while'.");
            Expr condition = Expression();
            Consume(TokenType.Right_Paren, "Expect ')' after condition.");
            Stmt body = Statement();

            return new While(condition, body);
        }

        private List<Stmt> Block()
        {
            List<Stmt> statements = new List<Stmt>();

            while (!Check(TokenType.Right_Brace) && !IsAtEnd())
            {
                Stmt stmt = Declaration();
                if (stmt != null)
                {
                    statements.Add(stmt);
                }
            }

            Consume(TokenType.Right_Brace, "Expect '}' after block.");
            return statements;
        }


        private Expr Expression()
        {
            return Assignment();
        }

        private Expr Assignment()
        {
            Expr expr = Or();

            if (Match(TokenType.Equal))
            {
                Token equals = Previous();
                Expr value = Assignment();

                if (expr is Variable variable)
                {
                    Token name = variable.Name;
                    return new Assign(name, value);
                }

                Error(equals, "Invalid assignment target.");
            }

            return expr;
        }

        private Expr Or()
        {
            Expr expr = And();

            while (Match(TokenType.Or))
            {
                Token operatorToken = Previous();
                Expr right = And();
                expr = new Logical(expr, operatorToken, right);
            }

            return expr;
        }

        private Expr And()
        {
            Expr expr = Equality();

            while (Match(TokenType.And))
            {
                Token operatorToken = Previous();
                Expr right = Equality();
                expr = new Logical(expr, operatorToken, right);
            }

            return expr;
        }

        private Expr Equality()
        {
            Expr expr = Comparison();

            while (Match(TokenType.Equal_Equal, TokenType.Not_Equal))
            {
                Token operatorToken = Previous();
                Expr right = Comparison();
                expr = new Binary(expr, operatorToken, right);
            }

            return expr;
        }

        private Expr Comparison()
        {
            Expr expr = Term();

            while (Match(TokenType.Less, TokenType.Less_Equal, TokenType.Greater, TokenType.Greater_Equal))
            {
                Token operatorToken = Previous();
                Expr right = Term();
                expr = new Binary(expr, operatorToken, right);
            }

            return expr;
        }

        private Expr Term()
        {
            Expr expr = Factor();

            while (Match(TokenType.Plus, TokenType.Minus, TokenType.Concat, TokenType.WhiteSpace_Concat))
            {
                Token operatorToken = Previous();
                Expr right = Factor();
                expr = new Binary(expr, operatorToken, right);
            }

            return expr;
        }

        private Expr Factor()
        {
            Expr expr = Unary();

            while (Match(TokenType.Multiplication, TokenType.Division, TokenType.Pow))
            {
                Token operatorToken = Previous();
                Expr right = Unary();
                expr = new Binary(expr, operatorToken, right);
            }

            return expr;
        }

        private Expr Unary()
        {
            if (Match(TokenType.Minus))
            {
                Token operatorToken = Previous();
                Expr right = Unary();
                return new Unary(operatorToken, right);
            }

            return Primary();
        }

        private Expr Primary()
        {
            if (Match(TokenType.False)) return new Literal(false);
            if (Match(TokenType.True)) return new Literal(true);
            if (Match(TokenType.Null)) return new Literal(null);

            if (Match(TokenType.Number))
            {
                return new Literal(Previous().Value);
            }

            if (Match(TokenType.String))
            {
                return new Literal(Previous().Value.Substring(1, Previous().Value.Length - 2));
            }

            if (Match(TokenType.Identifier))
            {
                return new Variable(Previous());
            }

            if (Match(TokenType.Left_Paren))
            {
                Expr expr = Expression();
                Consume(TokenType.Right_Paren, "Expect ')' after expression.");
                return new Grouping(expr);
            }

            throw Error(Peek(), "Expect expression.");
        }

    
        private bool Match(params TokenType[] types)
        {
            foreach (TokenType type in types)
            {
                if (Check(type))
                {
                    Advance();
                    return true;
                }
            }
            return false;
        }

        private bool Check(TokenType type)
        {
            if (IsAtEnd()) return false;
            return Peek().Type == type;
        }

        private Token Advance()
        {
            if (!IsAtEnd()) current++;
            return Previous();
        }

        private bool IsAtEnd()
        {
            return Peek().Type == TokenType.EOF;
        }

        private Token Peek()
        {
            return tokens[current];
        }

        private Token Previous()
        {
            return tokens[current - 1];
        }

        private Token Consume(TokenType type, string message)
        {
            if (Check(type)) return Advance();
            throw Error(Peek(), message);
        }

        private void Synchronize()
        {
            Advance();

            while (!IsAtEnd())
            {
                if (Previous().Type == TokenType.Semicolon) return;
                Advance();
            }
        }

        private ParseError Error(Token token, string message)
        {
            return new ParseError();
        }

        private class ParseError : Exception { }
    }
}
