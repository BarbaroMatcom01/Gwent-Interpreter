namespace Interpreter
{
    public partial class Parser
    {
        private Expr DotChainExpressions()
        {
            Expr left = Primary();

            while (Check(TokenType.Dot) || Check(TokenType.Left_Brackets) || Check(TokenType.Plus_Plus) || Check(TokenType.Minus_Minus))
            {
                if (MatchPrefix(TokenType.Dot, TokenType.Identifier, TokenType.Left_Paren))
                {
                    left = FunctionCall(left);
                }
                else if (MatchPrefix(TokenType.Dot, TokenType.Identifier))
                {
                    left = PropertyGetter(left);
                }
                else if (MatchPrefix(TokenType.Plus_Plus))
                {
                    left = Increment(left);
                }
                else if (MatchPrefix(TokenType.Minus_Minus))
                {
                    left = Decremet(left);
                }
                else
                {
                    left = Indexer(left);
                }
            }
            return left;
        }

        private Expr FunctionCall(Expr expr)
        {
            Consume(TokenType.Dot, "Expected '.'");
            string id = Consume(TokenType.Identifier, "Expected 'identifier'").Value;
            List<Expr> args;
            Consume(TokenType.Left_Paren, "Expected '('");
            if (Check(TokenType.Right_Paren))
            {
                args = new();
            }
            else
            {
                args = new()
                {
                    Expression()
                };
                while (Match(TokenType.Comma))
                {
                    args.Add(Expression());
                }

            }
            Consume(TokenType.Right_Paren, "Expected ')'");
            return new FunctionCall(expr, id, args);
        }
        private Expr PropertyGetter(Expr expr)
        {
            Consume(TokenType.Dot, "Expected '.'");
            string id = Consume(TokenType.Identifier, "Expected 'identifier'").Value;
            return new PropertyGetter(expr, id);
        }
        private Expr Increment(Expr expr)
        {
            var tok = Consume(TokenType.Plus_Plus, "Expected '++' after expression.");
            return new Unary(new Token(TokenType.Plus_Plus, "++", tok.Line, tok.Column), expr);
        }
        private Expr Decremet(Expr expr)
        {
            var tok = Consume(TokenType.Minus_Minus, "Expected '--' after expression.");
            return new Unary(new Token(TokenType.Minus_Minus, "--", tok.Line, tok.Column), expr);
        }
        private Expr Indexer(Expr expr)
        {
            Consume(TokenType.Left_Brackets, "Expected '['");
            Expr exp = Expression();
            Consume(TokenType.Right_Brackets, "Expected ']'");
            return new PropertyGetter(expr, "Indexer", new List<Expr> { exp });
        }
    }
}