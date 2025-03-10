public class GroupComparisonConditionItem : IConditionItem
{
    public List<IExpressionItem> leftExpressionItems { get; set; } = [];
    public string? comparisonOperator { get; set; }
    public string? groupModifier { get; set; }
    public List<List<IExpressionItem>> rightExpressionItems { get; set; } = [];
    public Query? subquery { get; set; }
}