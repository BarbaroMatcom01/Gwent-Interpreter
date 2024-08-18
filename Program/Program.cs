using System;
using System.Collections.Generic;

namespace Interpreter
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Ingrese el código a interpretar:");
                string input = Console.ReadLine();

                List<Token> listTokens = Lexer.LexicalAnalysis(input);
                foreach (var token in listTokens)
                {
                    Console.WriteLine($"El token es {token.Type} {token.Value} {token.Line} {token.Column}");
                }

                Parser parser = new Parser(listTokens);
                List<Stmt> statements = parser.Parse();

                Interpreter interpreter = new Interpreter();
                interpreter.Interpret(statements);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}