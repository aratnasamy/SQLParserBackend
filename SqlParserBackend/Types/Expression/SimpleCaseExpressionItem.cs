public class SimpleCaseExpressionItem : IExpressionItem
{
    public IExpressionItem? expression { get; set; }
    public List<(IExpressionItem,IExpressionItem)> pairs { get; set; } = [];
}