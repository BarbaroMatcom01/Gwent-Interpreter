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

        public Expr Parse()
        {
            try
            {
                return Expression();
            }
            catch (ParseError error)
            {
                return null;
            }
        }

        private Expr Expression()
        {
            return Term();
        }

        private Expr Term()
        {
            Expr expr = Factor();
            while (Match(TokenType.Plus, TokenType.Minus,TokenType.Pow,TokenType.Less,TokenType.Less_Equal,TokenType.Greater,TokenType.Greater_Equal,TokenType.Equal_Equal,TokenType.Not_Equal,TokenType.Concat,TokenType.WhiteSpace_Concat))
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
            while (Match(TokenType.Multiplication, TokenType.Division))
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
           
            if (Match( TokenType.String))
            {   
                return new Literal(Previous().Value.Substring(1,Previous().Value.Length-2));
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

        private ParseError Error(Token token, string message)
        {
            Error(token, message);
            return new ParseError();
        }

        private class ParseError : Exception { }
    }
}