
namespace Interpreter
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string filePath = @"C:\Users\Barbaro\Documents\Personal\Estudio\Pro\Proyectos Pro\Proyectos de la escuela\Proyecto (GWENT)\Gwent Interpreter\Gwent Interpreter\Input\input.txt";
                string input = File.ReadAllText(filePath);

                List<Token> listTokens = Lexer.LexicalAnalysis(input);
                foreach (var token in listTokens)
                {
                    Console.WriteLine($"El token es {token.Type} {token.Value} {token.Line} {token.Column}");
                }

                Parser parser = new Parser(listTokens);
                List<Stmt> statements = parser.Parse();

                Interpreter interpreter = new Interpreter();
                interpreter.Interpret(statements);

                List<InterpretedCard> cards = interpreter.GetCards();

                foreach (var card in cards)
                {
                    Console.WriteLine($"Carta: {card.Name}, Tipo: {card.Type}, Facción: {card.Faction}, Rango: {card.Range}, Poder: {card.Power}");
                }

                List<InterpretedEffect> effects = interpreter.GetEffects();
                foreach (var effect in effects)
                {
                    Console.WriteLine($"Effect Name: {effect.Name}");
                    foreach (var param in effect.Params)
                    {
                        Console.WriteLine($"  Param: {param.Key}, Type: {param.Value}");
                    }
                   
                    effect.Action.InvokeAction(new List<object>(){1,2,3},new List<object>(){1,2});
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

        }
    }
}