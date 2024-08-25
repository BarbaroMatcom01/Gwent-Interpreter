namespace Interpreter
{
    public class OnActivationExpr : Expr
    {
        public EffectInfoExpr EffectInfoExpr { get; }
        public SelectorExpr SelectorExpr { get; }
        public OnActivationExpr OnActivation { get; }
        public OnActivationExpr(EffectInfoExpr effectInfoExpr, SelectorExpr selectorExpr, OnActivationExpr onActivationExpr)
        {
            EffectInfoExpr = effectInfoExpr;
            SelectorExpr = selectorExpr;
            OnActivation = onActivationExpr;
        }
        public override T Accept<T>(IVisitorExp<T> visitor)
        {
            return visitor.VisitOnActivationExpr(this);
        }
    }

    public class EffectInfoExpr : Expr
    {
        public Expr Name { get; }
        public Dictionary<string, Expr> Param { get; }
        public EffectInfoExpr(Expr name, Dictionary<string, Expr> param)
        {
            Name = name;
            Param = param;
        }
        public override T Accept<T>(IVisitorExp<T> visitor)
        {
            return visitor.VisitEffectInfoExpr(this);
        }
    }
    public class SelectorExpr : Expr
    {
        public Expr Single { get; }
        public Expr Source { get; }
        public DelegateExpr DelExpr { get; }
        public SelectorExpr(Expr single, Expr source, DelegateExpr delegateExpr)
        {
            Single = single;
            Source = source;
            DelExpr = delegateExpr;
        }
        public override T Accept<T>(IVisitorExp<T> visitor)
        {
            return visitor.VisitSelectorExpr(this);
        }
    }

    public class DelegateExpr : Expr
    {
        public List<string> ParmasDelegate { get; }
        public Expr Expr { get; }
        public Environment Environment { get; }
        public DelegateExpr(List<string> parmasDelegate, Expr expr, Environment environment)
        {
            ParmasDelegate = parmasDelegate;
            Expr = expr;
            Environment = environment;
        }
        public override T Accept<T>(IVisitorExp<T> visitor)
        {
            return visitor.VisitDelegateExpr(this);
        }
    }
}