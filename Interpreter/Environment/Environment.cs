namespace Interpreter
{
    public class Environment
    {
        private readonly Environment enclosing;
        private readonly Dictionary<string, object> values = new Dictionary<string, object>();

        public Environment()
        {
            this.enclosing = null;
        }

        public Environment(Environment enclosing)
        {
            this.enclosing = enclosing;
        }

        public void Define(string name, object value)
        {
            values.Add(name,value);
        }

        public object Get(Token name)
        {
            if (values.TryGetValue(name.Value, out object value))
            {
                return value;
            }

            if (enclosing != null) return enclosing.Get(name);

            throw new RuntimeError(name, $"Undefined variable '{name.Value}'.");
        }

        public void Assign(Token name, object value)
        {
            if (values.ContainsKey(name.Value))
            {
                values[name.Value] = value;
                return;
            }

            if (enclosing != null)
            {
                enclosing.Assign(name, value);
                return;
            }
            throw new RuntimeError(name, $"Undefined variable '{name.Value}'.");
        }
    }
}