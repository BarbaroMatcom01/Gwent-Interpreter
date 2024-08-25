using System.Security.Cryptography;

namespace Interpreter
{
    public partial class Interpreter
    {
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

        public object VisitExpressionStmt(ExpressionStmt stmt)
        {
            Evaluate(stmt.ExpressionExpr);
            return null;
        }

        public object VisitBlockStmt(Block stmt)
        {
            var x = new Environment(environment);
            stmt.environment = x;
            ExecuteBlock(stmt.Statements, x);
            return null;
        }

        public object VisitPrintStmt(Print stmt)
        {
            object value = Evaluate(stmt.ExpressionExpr);
            Console.WriteLine(Stringify(value));
            return null;
        }

        public object VisitWhileStmt(While stmt)
        {
            while (IsTruthy(Evaluate(stmt.Condition)))
            {
                Execute(stmt.Body);
            }

            return null;
        }

        public object VisitForStmt(For stmt)
        {
            object collection = Evaluate(stmt.Collection);

            if (collection is IEnumerable<object> enumerable)
            {
                int i = 0;
                foreach (var item in enumerable)
                {

                    if (i == 0)
                    {
                        environment.Define(stmt.Variable.Value, item);
                        i++;
                    }
                    else
                    {
                        environment.Assign(stmt.Variable, item);
                    }
                    Execute(stmt.Body);
                }
            }
            else
            {
                throw new RuntimeError(stmt.Variable, "Foreach loop requires a collection.");
            }

            return null;
        }
    }
}