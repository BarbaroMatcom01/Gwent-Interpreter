namespace Interpreter
{
    public class PropertySetter : Expr
    {
        public Expr Left { get; }
        public string PropertyName { get; }
        public Expr Value { get; }
        public List<Expr> Args { get; }
        public PropertySetter(Expr left, string propertyName,Expr value, List<Expr> args=null)
        {
            Left = left;
            PropertyName = propertyName;
            Value = value;
            Args = args;
        }
        public override T Accept<T>(IVisitorExp<T> visitor)
        {
            return visitor.VisitPropertySetter(this);
        }
    }
}