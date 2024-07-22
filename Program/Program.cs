namespace Interpreter;
class Program
{
    static void Main(string[] args)
    {
        string input = Console.ReadLine();

        List<Token> listTokens = new List<Token>();
        listTokens = Lexer.LexicalAnalysis(input);
        foreach (var token in listTokens)
        {
            Console.WriteLine($"El token es {token.Type} {token.Value} {token.Line} {token.Column}");
        }
      
    }
}