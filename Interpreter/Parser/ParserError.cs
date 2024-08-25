namespace Interpreter
{
    public partial class Parser
    {
        private class ParseError : Exception { }
        private ParseError Error(Token token, string message)
        {
            return new ParseError();
        }
    }
}