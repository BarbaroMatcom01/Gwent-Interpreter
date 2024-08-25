namespace Interpreter
{
    public class EffectStmt : Stmt
    {
        public Token Name { get; }
        public Expr NameValue { get; }
        public List<Token> ParamsTokens { get; }
        public List<Token> ParamsValues { get; }
        public List<Token> ActionParams { get; }
        public Block ActionBlock { get; }

        public EffectStmt(Token name, Expr nameValue, List<Token> paramsToken, List<Token> paramsValues, List<Token> actionParams, Block actionBlock)
        {
            Name = name;
            NameValue = nameValue;
            ParamsTokens = paramsToken;
            ParamsValues = paramsValues;
            ActionParams = actionParams;
            ActionBlock = actionBlock;
        }

        public override T Accept<T>(IVisitorStmt<T> visitor)
        {
            return visitor.VisitEffectStmt(this);
        }
    }
}