namespace Interpreter
{
    public partial class Parser
    {
        private bool Match(params TokenType[] types)
        {
            foreach (TokenType type in types)
            {
                if (Check(type))
                {
                    Console.WriteLine("Matched token: " + type);
                    Advance();
                    return true;
                }
            }
            return false;
        }

        private bool MatchPrefix(params TokenType[] types)
        {
            for (int i = 0; i < types.Length; i++)
            {

                if (i + current >= tokens.Count || tokens[i + current].Type != types[i])
                {
                    return false;
                }
            }
            return true;
        }

        private void ParseProperty(ref Token propertyToken, ref Expr propertyValue, string propertyName)
        {
            propertyToken = Previous();
            Consume(TokenType.Colon, "Expected ':' after properties" + propertyName);
            propertyValue = Expression();
            Consume(TokenType.Comma, "Expected ':' after properties" + propertyName);
            Console.WriteLine("Parsed " + propertyName + ": " + propertyToken.Value);
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
            var x = Peek();
            throw Error(x, message);
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
    }
}