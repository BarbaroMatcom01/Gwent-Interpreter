namespace Interpreter
{
    public class Lexer
    {
        private static readonly List<Token> listTokens = new List<Token>();

        public static List<Token> LexicalAnalysis(string input)
        {
            string[] linesOfInput = input.Split('\n');
            LexerError lexerErrorInstance = new LexerError();

            if (lexerErrorInstance.LexerErrors == null)
            {
                lexerErrorInstance.LexerErrors = new List<Error>();
            }

            for (int i = 0; i < linesOfInput.Length; i++)
            {
                bool lastline = (i == linesOfInput.Length - 1);
                LexicalAnalysis(linesOfInput[i], i + 1, lastline, lexerErrorInstance);
            }

            if (lexerErrorInstance.LexerErrors.Any())
            {
                Console.WriteLine("Errors were found during lexical analysis:");
                foreach (var error in lexerErrorInstance.LexerErrors)
                {
                    Console.WriteLine($"Line {error.Line}, Column {error.Column}: {error.Value} {error.Messege}");
                }
                return new List<Token>();
            }

            return listTokens;
        }

        public static void LexicalAnalysis(string input, int line, bool lastline, LexerError lexerErrorInstance)
        {
            int column = 0;
            while (column < input.Length)
            {
                bool matched = false;
                if (char.IsWhiteSpace(input[column]))
                {
                    column++;
                    continue;
                }
                if (input[column] == '/' && column + 1 < input.Length && input[column + 1] == '/')
                {
                    return;
                }

                foreach (var token in Token.TokenStringDictionary)
                {
                    var match = token.Value.Match(input.Substring(column));
                    if (match.Success)
                    {
                        listTokens.Add(new Token(token.Key, match.Groups[0].Value, line, column + 1));
                        column += match.Length;
                        matched = true;
                        break;
                    }
                }

                if (!matched)
                {
                    lexerErrorInstance.LexerErrors.Add(new Error(input[column].ToString(), line, column, "Unsupported token"));
                    column++;
                }
            }

            if (lastline && column == input.Length)
            {
                listTokens.Add(new Token(TokenType.EOF, "", line, column + 1));
            }
        }
    }
}