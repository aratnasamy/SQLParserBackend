public class NullConditionItem : IConditionItem
{
    public IExpressionItem? conditionExpression { get; set; }
    public string? nullCondition { get; set; }
}