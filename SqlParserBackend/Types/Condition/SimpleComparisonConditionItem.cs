public class SimpleComparisonConditionItem : IConditionItem
{
    public List<IExpressionItem> leftExpressionItems { get; set; } = [];
    public string? comparisonOperator { get; set; }
    public List<IExpressionItem> rightExpressionItems { get; set; } = [];
    public Query? subquery { get; set; }
}