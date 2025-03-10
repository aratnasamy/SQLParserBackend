public class BetweenConditionItem : IConditionItem
{
    public IExpressionItem? expression1 { get; set; }
    public string? logic { get; set; }
    public IExpressionItem? expression2 { get; set; }
    public IExpressionItem? expression3 { get; set; }
}