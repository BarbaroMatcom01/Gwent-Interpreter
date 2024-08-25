namespace Interpreter
{
    public class PropertyGetter : Expr
    {
        public Expr Left { get; }
        public string PropertyName { get; }
        public List<Expr> Args { get; }
        public PropertyGetter(Expr left, string propertyName, List<Expr> args=null)
        {
            Left = left;
            PropertyName = propertyName;
            Args = args;
        }
        public override T Accept<T>(IVisitorExp<T> visitor)
        {
            return visitor.VisitPropertyGetter(this);
        }
    }
}