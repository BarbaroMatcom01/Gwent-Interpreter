namespace Interpreter
{
    public partial class Interpreter
    {
        public object VisitLogicalExpr(Logical expr)
        {
            object left = Evaluate(expr.Left);

            if (expr.Operator.Type == TokenType.Or)
            {
                if (IsTruthy(left)) return true;
            }
            else if (expr.Operator.Type == TokenType.And)
            {
                if (!IsTruthy(left)) return false;
            }

            object right = Evaluate(expr.Right);

            return IsTruthy(right);
        }
        public object VisitVariableExpr(Variable expr)
        {
            return environment.Get(expr.Name);
        }

        public object VisitAssignExpr(Assign expr)
        {
            object value = Evaluate(expr.Value);
            environment.Assign(expr.Name, value);
            return value;
        }
        public object VisitGroupingExpr(Grouping expr)
        {
            return Evaluate(expr.Expression);
        }

        public object VisitLiteralExpr(Literal expr)
        {
            return expr.Value;
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

        public object VisitUnaryExpr(Unary expr)
        {
            object right = Evaluate(expr.Right);

            switch (expr.Operator.Type)
            {
                case TokenType.Minus:
                    CheckNumberOperand(expr.Operator, right);
                    return -int.Parse(right.ToString());

                case TokenType.Minus_Minus:
                    CheckNumberOperand(expr.Operator, right);
                    int valueMinus = int.Parse(right.ToString());
                    valueMinus--;
                    return valueMinus;

                case TokenType.Plus_Plus:
                    CheckNumberOperand(expr.Operator, right);
                    int valuePlus = int.Parse(right.ToString());
                    valuePlus++;
                    return valuePlus;
                default:
                    return null;
            }
        }
    }
}