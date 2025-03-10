public class CaseExpressionItem : IExpressionItem
{
    public IExpressionItem? caseExpression { get; set; }
    public IExpressionItem? elseExpression { get; set; }
}