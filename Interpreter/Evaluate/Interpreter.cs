namespace Interpreter
{
    public class Interpreter : IVisitorExp<object>, IVisitorStmt<object>
    {

        private Environment environment = new Environment();

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

        public object VisitVariableExpr(Variable expr)
        {
            return environment.Get(expr.Name);
        }

        public object VisitVarStmt(Var stmt)
        {
            object value = null;
            if (stmt.Initializer != null)
            {
                value = Evaluate(stmt.Initializer);
            }

            environment.Define(stmt.Name.Value, value);
            return null;
        }

        private object Evaluate(Expr expr)
        {
            return expr.Accept(this);
        }

        private void Execute(Stmt stmt)
        {
            stmt.Accept(this);
        }
        public object VisitExpressionStmt(Expression stmt)
        {
            Evaluate(stmt.ExpressionExpr);
            return null;
        }
        public object VisitBlockStmt(Block stmt)
        {
            ExecuteBlock(stmt.Statements, new Environment(environment));
            return null;
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
        public object VisitPrintStmt(Print stmt)
        {
            object value = Evaluate(stmt.ExpressionExpr);
            Console.WriteLine(Stringify(value));
            return null;
        }

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
        public object VisitWhileStmt(While stmt)
        {
            while (IsTruthy(Evaluate(stmt.Condition)))
            {
                Execute(stmt.Body);
            }

            return null;
        }
        private bool IsTruthy(object value)
        {
            if (value == null) return false;
            if (value is bool boolValue) return boolValue;

            return true;
        }
        public object VisitAssignExpr(Assign expr)
        {
            object value = Evaluate(expr.Value);
            environment.Assign(expr.Name, value);
            return value;
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

    public class RuntimeError : Exception
    {
        public Token Token { get; }

        public RuntimeError(Token token, string message) : base(message)
        {
            Token = token;
        }
    }
}
