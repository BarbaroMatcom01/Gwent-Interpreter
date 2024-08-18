using System.Linq.Expressions;
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
    }

    public interface IVisitorStmt<T>
    {
        T VisitExpressionStmt(Expression stmt);
        T VisitPrintStmt(Print stmt);
        T VisitVarStmt(Var stmt);
        T VisitWhileStmt(While stmt);
        T VisitBlockStmt(Block stmt);
    }
}