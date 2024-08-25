namespace Interpreter
{
    public partial class Parser
    {
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
                else if (expr is PropertyGetter pg)
                {
                    return new PropertySetter(pg.Left,pg.PropertyName,value,pg.Args);
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
            if (Match(TokenType.Minus)||Match(TokenType.Minus_Minus)||Match(TokenType.Plus_Plus))
            {
                Token operatorToken = Previous();
                Expr right = Unary();
                return new Unary(operatorToken, right);
            }

            return DotChainExpressions();
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

            if(
                 MatchPrefix(TokenType.Left_Paren,TokenType.Right_Paren)||
                 MatchPrefix(TokenType.Left_Paren,TokenType.Identifier,TokenType.Right_Paren,TokenType.Lambda)||
                 MatchPrefix(TokenType.Left_Paren,TokenType.Identifier,TokenType.Comma)
                 )
            {
                 return DelegateEx();
            }
            if (Match(TokenType.Left_Paren))
            {
                Expr expr = Expression();
                Consume(TokenType.Right_Paren, "Expect ')' after expression.");
                return new Grouping(expr);
            }

            throw Error(Peek(), "Expect expression.");
        }
    }
}