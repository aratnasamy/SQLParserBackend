public class InConditionItem : IConditionItem
{
    public List<IExpressionItem> leftExpressionItem { get; set; } = [];
    public string? logic { get; set; }
    public List<List<IExpressionItem>> rightExpressionItems { get; set; } = [];
    public Query? subQuery { get; set; }
}