public class ExpressionItem : IExpressionItem
{
    public List<IExpressionItem> expressionItems { get; set; } = [];
    public override string ToString()
    {
        string res = "";
        foreach (IExpressionItem expressionItem in expressionItems) {
            res += expressionItem.ToString();
        }
        return res;
    }
}