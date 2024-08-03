namespace Interpreter
{
    public abstract class Expr
    {
        public abstract T Accept<T>(IVisitor<T> visitor);
    }

    public class Binary : Expr
    {
        public Binary(Expr left, Token op, Expr right)
        {
            Left = left;
            Operator = op;
            Right = right;
        }

        public Expr Left { get; }
        public Token Operator { get; }
        public Expr Right { get; }

        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.VisitBinaryExpr(this);
        }
    }

    public class Grouping : Expr
    {
        public Grouping(Expr expression)
        {
            Expression = expression;
        }

        public Expr Expression { get; }

        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.VisitGroupingExpr(this);
        }
    }

    public class Literal : Expr
    {
        public Literal(object value)
        {
            Value = value;
        }

        public object  Value { get; }

        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.VisitLiteralExpr(this);
        }
    }

    public class Unary : Expr
    {
        public Unary(Token op, Expr right)
        {
            Operator = op;
            Right = right;
        }

        public Token Operator { get; }
        public Expr Right { get; }

        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.VisitUnaryExpr(this);
        }
    }
}