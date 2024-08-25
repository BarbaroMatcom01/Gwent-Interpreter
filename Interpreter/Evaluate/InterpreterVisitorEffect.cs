namespace Interpreter
{
    public partial class Interpreter
    {
        public object VisitEffectStmt(EffectStmt stmt)
        {
            string name = (string)Evaluate(stmt.NameValue);

            Dictionary<string, object> @params = new Dictionary<string, object>();
            for (int i = 0; i < stmt.ParamsTokens.Count; i++)
            {
                string paramName = stmt.ParamsTokens[i].Value;
                object paramValue = stmt.ParamsValues[i].Value;
                @params[paramName] = paramValue;
            }

            if (stmt.ActionParams == null)
            {
                throw new ArgumentNullException(nameof(stmt.ActionParams), "ActionParams cannot be null.");
            }

            if (stmt.ActionBlock == null)
            {
                throw new ArgumentNullException(nameof(stmt.ActionBlock), "ActionBlock cannot be null.");
            }

            Action action = new Action(stmt.ActionParams, stmt.ActionBlock);

            InterpretedEffect effect = new InterpretedEffect(name, @params, action);

            effects.Add(effect);
            return null;
        }

        public object VisitFunctionCall(FunctionCall expr)
        {
            object l = Evaluate(expr.LeftExpression);
            if (l is List<object> objectList)
            {
                switch (expr.function)
                {
                    case "Remove":
                        return objectList.Remove(Evaluate(expr.args[0]));
                    case "Push":
                        objectList.Add(Evaluate(expr.args[0]));
                        return typeof(void);
                    case "Pop":
                        var item = objectList[^1];
                        objectList.RemoveAt(objectList.Count - 1);
                        return item;
                    default:
                        throw new Exception();
                }
            }
            return null;
        }

        public object VisitPropertyGetter(PropertyGetter expr)
        {
            object l = Evaluate(expr.Left);
            if (l is List<object> objectList)
            {
                switch (expr.PropertyName)
                {
                    case "Count":
                        return objectList.Count;
                    case "Indexer":
                        return objectList[Convert.ToInt32(Evaluate(expr.Args[0]))];
                    default:
                        throw new Exception();
                }
            }
            return null;
        }

        public object VisitPropertySetter(PropertySetter expr)
        {
            object l = Evaluate(expr.Left);
            if (l is List<object> objectList)
            {
                switch (expr.PropertyName)
                {

                    case "Indexer":
                        var value = Evaluate(expr.Value);
                        objectList[Convert.ToInt32(Evaluate(expr.Args[0]))] = value;
                        return value;
                    default:
                        throw new Exception();
                }
            }
            return null;
        }
    }
}