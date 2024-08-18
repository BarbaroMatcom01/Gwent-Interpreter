namespace Interpreter
{
    public abstract class Stmt
    {
        public abstract T Accept<T>(IVisitorStmt<T> visitor);
    }

    public class Expression : Stmt
    {
        public readonly Expr ExpressionExpr;

        public Expression(Expr expression)
        {
            this.ExpressionExpr = expression;
        }
        public override T Accept<T>(IVisitorStmt<T> visitor)
        {
            return visitor.VisitExpressionStmt(this);
        }
    }
    public class Print : Stmt
    {
        public readonly Expr ExpressionExpr;

        public Print(Expr expression)
        {
            this.ExpressionExpr = expression;
        }
        public override T Accept<T>(IVisitorStmt<T> visitor)
        {
            return visitor.VisitPrintStmt(this);
        }
    }
    public class Var : Stmt
    {
        public Token Name { get; }
        public Expr Initializer { get; }

        public Var(Token name, Expr initializer)
        {
            Name = name;
            Initializer = initializer;
        }

        public override T Accept<T>(IVisitorStmt<T> visitor)
        {
            return visitor.VisitVarStmt(this);
        }
    }
    public class While : Stmt
    {
        public readonly Expr Condition;
        public readonly Stmt Body;

        public While(Expr condition, Stmt body)
        {
            this.Condition = condition;
            this.Body = body;
        }

        public override T Accept<T>(IVisitorStmt<T> visitor)
        {
            return visitor.VisitWhileStmt(this);
        }
    }
    public class Block : Stmt
    {
        public readonly List<Stmt> Statements;

        public Block(List<Stmt> statements)
        {
            this.Statements = statements;
        }

        public override R Accept<R>(IVisitorStmt<R> visitor)
        {
            return visitor.VisitBlockStmt(this);
        }
    }
}