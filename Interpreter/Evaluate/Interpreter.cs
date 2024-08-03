namespace Interpreter
{
    public class Interpreter : IVisitor<object>
    {
        public object VisitLiteralExpr(Literal expr)
        {
            return expr.Value;
        }

        public object VisitGroupingExpr(Grouping expr)
        {
            return Evaluate(expr.Expression);
        }

        public object VisitUnaryExpr(Unary expr)
        {
            object right = Evaluate(expr.Right);

            switch (expr.Operator.Type)
            {
                case TokenType.Minus:
                    CheckNumberOperand(expr.Operator, right);
                    return -int.Parse(right.ToString());
                default:
                    return null;
            }
        }

        public object VisitBinaryExpr(Binary expr)
        {
            var left = Evaluate(expr.Left);
            var right = Evaluate(expr.Right);

            switch (expr.Operator.Type)
            {
                case TokenType.Minus:
                    CheckNumberOperand(expr.Operator, left, right);
                    return int.Parse(left.ToString()) - int.Parse(right.ToString());

                case TokenType.Division:
                    CheckNumberOperand(expr.Operator, left, right);
                    if (int.Parse(right.ToString()) == 0) throw new RuntimeError(expr.Operator, "Division by zero.");
                    return int.Parse(left.ToString()) / int.Parse(right.ToString());

                case TokenType.Multiplication:
                    CheckNumberOperand(expr.Operator, left, right);
                    return int.Parse(left.ToString()) * int.Parse(right.ToString());

                case TokenType.Pow:
                    CheckNumberOperand(expr.Operator, left, right);
                    return Math.Pow(int.Parse(left.ToString()), int.Parse(right.ToString()));

                case TokenType.Plus:
                    CheckNumberOperand(expr.Operator, left, right);
                    return int.Parse(left.ToString()) + int.Parse(right.ToString());
                    throw new RuntimeError(expr.Operator, "Operands must be two numbers or two strings.");

                case TokenType.Greater:
                    CheckNumberOperand(expr.Operator, left, right);
                    return int.Parse(left.ToString()) > int.Parse(right.ToString());

                case TokenType.Greater_Equal:
                    CheckNumberOperand(expr.Operator, left, right);
                    return int.Parse(left.ToString()) >= int.Parse(right.ToString());

                case TokenType.Less:
                    CheckNumberOperand(expr.Operator, left, right);
                    return int.Parse(left.ToString()) < int.Parse(right.ToString());

                case TokenType.Less_Equal:
                    CheckNumberOperand(expr.Operator, left, right);
                    return int.Parse(left.ToString()) <= int.Parse(right.ToString());

                case TokenType.Not_Equal:
                    return !IsEqual(left, right);

                case TokenType.Equal_Equal:
                    return IsEqual(left, right);

                case TokenType.Concat:
                    CheckStringOperand(expr.Operator, left, right);
                    return left.ToString() + right.ToString();

                case TokenType.WhiteSpace_Concat:
                    CheckStringOperand(expr.Operator, left, right);
                    return left.ToString() + " " + right.ToString();

                default:
                    return null;
            }
        }
        public object Evaluate(Expr expr)
        {
            return expr.Accept(this);
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
    }

    public class RuntimeError : Exception
    {
        public Token Token { get; }

        public RuntimeError(Token token, string message) : base(message)
        {
            Token = token;
        }
    }
}