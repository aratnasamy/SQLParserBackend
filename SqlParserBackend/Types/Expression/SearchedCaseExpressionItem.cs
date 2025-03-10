public class SearchedCaseExpressionItem : IExpressionItem
{
    public List<(IConditionItem,IExpressionItem)> pairs { get; set; } = [];
}