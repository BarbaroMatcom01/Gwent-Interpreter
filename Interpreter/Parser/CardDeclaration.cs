
namespace Interpreter
{
    public partial class Parser
    {
        private Stmt CardDeclaration()
        {
            Token type = null;
            Expr typeValue = null;

            Token name = null;
            Expr nameValue = null;

            Token faction = null;
            Expr factionValue = null;

            Token power = null;
            Expr powerValue = null;

            Token range = null;
            Expr rangeValue = null;

            List<OnActivationExpr> onActivationExprs = new();

            Console.WriteLine("Starting CardDeclaration");
            Consume(TokenType.Left_Brace, "Expected '{' after 'card'.");

            while (!Check(TokenType.Right_Brace) && !IsAtEnd())
            {
                if (Match(TokenType.Type))
                {
                    ParseProperty(ref type, ref typeValue, "Type");
                }
                else if (Match(TokenType.Name))
                {
                    ParseProperty(ref name, ref nameValue, "Name");
                }
                else if (Match(TokenType.Faction))
                {
                    ParseProperty(ref faction, ref factionValue, "Faction");
                }
                else if (Match(TokenType.Power))
                {
                    ParseProperty(ref power, ref powerValue, "Power");
                }
                else if (Match(TokenType.Range))
                {
                    ParseProperty(ref range, ref rangeValue, "Range");
                }
                else if (Match(TokenType.OnActivation))
                {
                    Consume(TokenType.Colon, "Expected ':' after 'OnActivation'.");
                    Consume(TokenType.Left_Brackets, "Expected '[' after ':' in 'OnActivation'.");
                    while (Match(TokenType.Left_Brace))
                    {
                        onActivationExprs.Add(ParseOnActivation());
                    }
                    Consume(TokenType.Right_Brace, "Expected '}' after 'OnActivation' block.");
                    Consume(TokenType.Right_Brackets, "Expected ']' after 'OnActivation' block.");
                }
                else
                {
                    throw new ParseError();
                }
            }

            Consume(TokenType.Right_Brace, "Expected '}' after card properties.");
            Console.WriteLine("Finished CardDeclaration");

            return new CardStmt(type, typeValue, name, nameValue, faction, factionValue, power, powerValue, range, rangeValue, onActivationExprs);
        }

        private OnActivationExpr ParseOnActivation()
        {
            EffectInfoExpr effectInfoExpr = null;
            SelectorExpr selectorExpr = null;
            OnActivationExpr onActivationExpr = null;

            while (Match(TokenType.Identifier))
            {
                if (Previous().Value == "Effect")
                {
                    Consume(TokenType.Colon, "Expected ':' after 'Effect'.");
                    Consume(TokenType.Left_Brace, "Expected '{' after ':' in 'Effect'.");
                    Consume(TokenType.Identifier, "Expected identifier in 'Effect'.");
                    Consume(TokenType.Colon, "Expected ':' after identifier in 'Effect'.");
                    Expr name = Expression();
                    Consume(TokenType.Comma, "Expected ',' after expression in 'Effect'.");

                    var param = ParseParmEffect();

                    effectInfoExpr = new EffectInfoExpr(name, param);
                    Consume(TokenType.Right_Brace, "Expected '}' after 'Effect' block.");

                    if (Check(TokenType.Comma))
                    {
                        Consume(TokenType.Comma, "Expected ',' after 'Effect' block.");
                    }
                }
                else if (Previous().Value == "Selector")
                {
                    Consume(TokenType.Colon, "Expected ':' after 'Selector'.");
                    Consume(TokenType.Left_Brace, "Expected '{' after ':' in 'Selector'.");
                    Expr source = null;
                    Expr single = null;
                    DelegateExpr del = null;

                    while (Match(TokenType.Identifier))
                    {
                        if (Previous().Value == "Source")
                        {
                            Consume(TokenType.Colon, "Expected ':' after 'Source'.");
                            source = Expression();
                            if (Check(TokenType.Comma))
                            {
                                Consume(TokenType.Comma, "Expected ',' after 'Source' expression.");
                            }
                        }
                        else if (Previous().Value == "Single")
                        {
                            Consume(TokenType.Colon, "Expected ':' after 'Single'.");
                            single = Expression();
                            if (Check(TokenType.Comma))
                            {
                                Consume(TokenType.Comma, "Expected ',' after 'Single' expression.");
                            }
                        }
                        else if (Previous().Value == "Predicate")
                        {
                            Consume(TokenType.Colon, "Expected ':' after 'Predicate'.");
                            del = (DelegateExpr)Expression();
                            if (Check(TokenType.Comma))
                            {
                                Consume(TokenType.Comma, "Expected ',' after 'Predicate' expression.");
                            }
                        }
                    }
                    Consume(TokenType.Right_Brace, "Expected '}' after 'Selector' block.");
                    selectorExpr = new SelectorExpr(single, source, del);
                }
                else if (Previous().Value == "PostAction")
                {
                }
            }
            return new OnActivationExpr(effectInfoExpr, selectorExpr, onActivationExpr);
        }

        private Dictionary<string, Expr> ParseParmEffect()
        {
            Dictionary<string, Expr> paramsEffect = new();
            while (Match(TokenType.Identifier))
            {
                Consume(TokenType.Colon, "Expected ':' after parameter name.");
                string key = Previous().Value;
                Expr value = Expression();
                paramsEffect.Add(key, value);
                if (Check(TokenType.Comma))
                {
                    Consume(TokenType.Comma, "Expected ',' after parameter value.");
                }
                else
                {
                    break;
                }
            }
            return paramsEffect;
        }

        private Expr DelegateEx()
        {
            List<string> id = new();
            Consume(TokenType.Left_Paren, "Expected '(' before delegate parameters.");

            if (MatchPrefix(TokenType.Right_Paren))
            {
                Consume(TokenType.Right_Paren, "Expected ')' after delegate parameters.");
            }
            else
            {
                id.Add(Consume(TokenType.Identifier, "Expected identifier in delegate parameters.").Value);

                while (Match(TokenType.Comma))
                {
                    id.Add(Consume(TokenType.Identifier, "Expected identifier after ',' in delegate parameters.").Value);
                }
            }
            Consume(TokenType.Right_Paren, "Expected ')' after delegate parameters.");
            Consume(TokenType.Lambda, "Expected '=>' after delegate parameters.");
            var e = Expression();

            Interpreter interpreter = new Interpreter();
            Environment delegateEnvironment = new Environment(interpreter.environment);

            return new DelegateExpr(id, e, delegateEnvironment);
        }
    }
}
