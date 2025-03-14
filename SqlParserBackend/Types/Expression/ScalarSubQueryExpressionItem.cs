public class ScalarSubQueryExpressionItem : IExpressionItem
{
    public Query? subQuery { get; set; }
    public override string ToString()
    {
        return subQuery.ToString();
    }
}