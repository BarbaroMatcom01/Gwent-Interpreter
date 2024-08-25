namespace Interpreter
{
    public class FunctionCall : Expr
    {
        public string function { get; }
        public Expr LeftExpression { get; }
        public List<Expr> args { get; }

        public FunctionCall(Expr leftExpression, string function, List<Expr> args)
        {
            this.LeftExpression = leftExpression;
            this.function = function;
            this.args = args;
        }

        public override T Accept<T>(IVisitorExp<T> visitor)
        {
            return visitor.VisitFunctionCall(this);
        }
    }
}