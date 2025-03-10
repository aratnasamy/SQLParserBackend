public class FunctionExpressionItem : IExpressionItem
{
    public string? function { get; set; }
    public List<IExpressionItem> parameters { get; set; } = [];
}