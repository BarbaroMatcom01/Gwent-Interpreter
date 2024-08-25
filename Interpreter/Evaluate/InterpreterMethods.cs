namespace Interpreter
{
    public partial class Interpreter
    {
        public void Interpret(List<Stmt> statements)
        {
            try
            {
                foreach (Stmt statement in statements)
                {
                    Execute(statement);
                }
            }
            catch (RuntimeError error)
            {
                ReportRuntimeError(error);
            }
        }

        public List<InterpretedCard> GetCards()
        {
            return cards;
        }
        public List<InterpretedEffect> GetEffects()
        {
            return effects;
        }
       
        public object Evaluate(Expr expr)
        {
            return expr.Accept(this);
        }

        public void Execute(Stmt stmt)
        {
            stmt.Accept(this);
        }

        public object ExecuteBlock(List<Stmt> statements, Environment environment)
        {
            Environment previous = this.environment;
            try
            {
                this.environment = environment;

                foreach (Stmt statement in statements)
                {
                    Execute(statement);
                }
            }
            finally
            {
                this.environment = previous;
            }
            return null;
        }

        private bool IsTruthy(object value)
        {
            if (value == null) return false;
            if (value is bool boolValue) return boolValue;

            return true;
        }

        private void CheckNumberOperand(Token operatorToken, object right)
        {
            if (right is int || (right is string rightStr && int.TryParse(rightStr, out _))) return;
            throw new RuntimeError(operatorToken, "Operand must be a number.");
        }

        private void CheckNumberOperand(Token operadorToken, object left, object right)
        {
            if ((left is int || (left is string leftStr && int.TryParse(leftStr, out _))) &&
                (right is int || (right is string rightStr && int.TryParse(rightStr, out _)))) return;
            throw new RuntimeError(operadorToken, "Operands must be numbers.");
        }

        private void CheckStringOperand(Token operadorToken, object left, object right)
        {
            bool IsStringAndNotNumber(object obj)
            {
                if (obj is string str)
                {
                    return !int.TryParse(str, out _);
                }
                return false;
            }

            if (IsStringAndNotNumber(left) && IsStringAndNotNumber(right)) return;
            throw new RuntimeError(operadorToken, "Operands must be string ");
        }

        private bool IsEqual(object a, object b)
        {
            if (a == null && b == null) return true;
            if (a == null) return false;
            return a.Equals(b);
        }

        private string Stringify(object obj)
        {
            if (obj == null) return "nil";

            if (obj is double doubleVal)
            {
                string text = doubleVal.ToString();
                if (text.EndsWith(".0"))
                {
                    text = text.Substring(0, text.Length - 2);
                }
                return text;
            }

            return obj.ToString();
        }

        private void ReportRuntimeError(RuntimeError error)
        {
            Console.WriteLine($"[line {error.Token.Line}] Error: {error.Message}");
        }
    }
}
