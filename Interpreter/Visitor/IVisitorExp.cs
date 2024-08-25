namespace Interpreter
{
    public interface IVisitorExp<T>
    {
        T VisitAssignExpr(Assign expr);
        T VisitLogicalExpr(Logical expr);
        T VisitLiteralExpr(Literal expr);
        T VisitVariableExpr(Variable expr);
        T VisitUnaryExpr(Unary expr);
        T VisitBinaryExpr(Binary expr);
        T VisitGroupingExpr(Grouping expr);

        T VisitFunctionCall(FunctionCall expr);
        T VisitPropertyGetter(PropertyGetter expr);
        T VisitPropertySetter(PropertySetter expr);

        T VisitOnActivationExpr(OnActivationExpr expr);
        T VisitEffectInfoExpr(EffectInfoExpr expr);
        T VisitSelectorExpr(SelectorExpr expr);
        T VisitDelegateExpr(DelegateExpr expr);

    }
}