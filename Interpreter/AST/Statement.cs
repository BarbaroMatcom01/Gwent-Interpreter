namespace Interpreter
{
    public abstract class Stmt
    {
        public abstract T Accept<T>(IVisitorStmt<T> visitor);
    }

    public class ExpressionStmt : Stmt
    {
        public Expr ExpressionExpr;

        public ExpressionStmt(Expr expression)
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
        public  Expr ExpressionExpr;

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
        public Expr Condition;
        public Stmt Body;

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
   
    public class For : Stmt
    {
        public Token Variable { get; }
        public Expr Collection { get; }
        public Stmt Body { get; }
        public For(Token variable, Expr collection, Stmt body)
        {
            Variable = variable;
            Collection = collection;
            Body = body;
        }
        public override T Accept<T>(IVisitorStmt<T> visitor)
        {
            return visitor.VisitForStmt(this);
        }
    }
   
    public class Block : Stmt
    {
        public List<Stmt> Statements;

        public Environment environment { get; set; }
        public Block(List<Stmt> statements)
        {
            this.Statements = statements;
        }
        public override T Accept<T>(IVisitorStmt<T> visitor)
        {
            return visitor.VisitBlockStmt(this);
        }
    }
}