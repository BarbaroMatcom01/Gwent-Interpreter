namespace Interpreter
{
    public partial class InterpretedEffect : InterpretedElement
    {
        public string Name { get; }
        public Dictionary<string, object> Params { get; }

        public Action Action { get; }

        public InterpretedEffect(string name, Dictionary<string, object> @params, Action action)
        {
            this.Name = name;
            this.Params = @params;
            this.Action = action;
        }
    }
}