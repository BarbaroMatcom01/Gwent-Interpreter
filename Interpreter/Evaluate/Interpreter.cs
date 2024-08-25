namespace Interpreter
{
    public partial class Interpreter : IVisitorExp<object>, IVisitorStmt<object>
    {
        public Environment environment = new Environment();
        private List<InterpretedCard> cards;
        private List<InterpretedEffect> effects;

        public Interpreter()
        {
            cards = new List<InterpretedCard>();
            effects = new List<InterpretedEffect>();
        }
    }
}