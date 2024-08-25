namespace Interpreter
{
    public partial class Action : InterpretedElement
    {
        public List<Token> ActionParams { get; }
       
        public Block ActionBlock { get; }
       
        public Action(List<Token> actionParams, Block actionBlock)
        {
            ActionParams = actionParams;
            ActionBlock = actionBlock;
        }
      
        public void InvokeAction(params object[] args)
        {
            Interpreter interpreter = new Interpreter();
            Environment actionEnvironment = new Environment(interpreter.environment);

            for (int i = 0; i < ActionParams.Count; i++)
            {
                actionEnvironment.Define(ActionParams[i].Value, args[i]);
            }
            interpreter.ExecuteBlock(ActionBlock.Statements, actionEnvironment);
        }
    }
}