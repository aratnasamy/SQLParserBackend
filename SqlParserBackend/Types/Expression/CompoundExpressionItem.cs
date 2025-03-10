public class CompoundExpressionItem : IExpressionItem
{
    public IExpressionItem? expression { get; set; }
    public string? expressionOperator { get; set; }
}