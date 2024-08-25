namespace Interpreter
{
    public partial class OnActivationObject : InterpretedElement
    {
        public EffectInfo Info { get; }
        public Selector Selector { get; }
        public OnActivationObject PostAction { get; }
        public OnActivationObject(EffectInfo info, Selector selector, OnActivationObject postAction)
        {
            Info = info;
            Selector = selector;
            PostAction = postAction;
        }
    }

    public partial class EffectInfo : InterpretedElement
    {
        public string Name { get; }
        public Dictionary<string, object> Param { get; }
        public EffectInfo(string name, Dictionary<string, object> param)
        {
            Name = name;
            Param = param;
        }
    }

    public partial class Selector : InterpretedElement
    {
        public bool Single { get; }
        public string Source { get; }
        public Delegate Delegate { get; }
        public Selector(bool single, string source, Delegate deleg)
        {
            Single = single;
            Source = source;
            Delegate = deleg;
    
        }
    }
    public partial class Delegate : InterpretedElement
    {
        public List<string> Param { get; }
        public Expr Expr { get; }
        public Environment Environment { get; }
        public Delegate(List<string> param, Expr expr, Environment environment)
        {
            Param = param;
            Expr = expr;
            Environment = environment;
        }


        public object InvokeDelegate(object[] parmas)
        {
            for (int i = 0; i < parmas.Length; i++)
            {
                Environment.Define(Param[i], parmas[i]);

            }
            Interpreter interpreter = new Interpreter();
            return interpreter.Evaluate(Expr);
        }
    }
}