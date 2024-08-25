namespace Interpreter
{
    public partial class Parser
    {
        private Stmt Statement()
        {
            if (Match(TokenType.Print)) return PrintStatement();
            if (Match(TokenType.Left_Brace)) return new Block(Block());
            if (Match(TokenType.While)) return WhileStatement();
            if (Match(TokenType.For)) return ForStatement();
            return ExpressionStatement();
        }
        private Stmt Declaration()
        {
            try
            {
                Console.WriteLine("Current token: " + Peek().Type);

                if (Match(TokenType.Var)) return VarDeclaration();
                if (Match(TokenType.Card)) return CardDeclaration();
                if (Match(TokenType.Effect)) return EffectDeclaration();

                return Statement();
            }
            catch (ParseError error)
            {
                Console.WriteLine("Parse error: " + error.Message);
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
            return new ExpressionStmt(expr);
        }

        private Stmt WhileStatement()
        {
            Consume(TokenType.Left_Paren, "Expect '(' after 'while'.");
            Expr condition = Expression();
            Consume(TokenType.Right_Paren, "Expect ')' after condition.");
            Stmt body = Statement();

            return new While(condition, body);
        }

        private Stmt ForStatement()
        {
            Consume(TokenType.Left_Paren, "Expected '(' after 'foreach'.");
            Token variable = Consume(TokenType.Identifier, "Expected variable name.");
            Consume(TokenType.In, "Expected 'in' after variable name.");
            Expr collection = Expression();
            Consume(TokenType.Right_Paren, "Expected ')' after collection.");
            Stmt body = Statement();

            return new For(variable, collection, body);
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
    }
}