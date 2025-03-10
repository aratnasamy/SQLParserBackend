public class Query
{
    public string? queryJoin { get; set; }
    public List<WithItem> withItems { get; set; } = [];
    public string? selectModifier { get; set; }
    public List<SelectItem> selectItems { get; set; } = [];
    public List<List<SourceItem>> sources { get; set; } = [];
    public IConditionItem? whereCondition { get; set; }
    public List<IExpressionItem> groupByExpressions { get; set; } = [];
    public IConditionItem? groupByCondition { get; set; }
    public string? orderByModifier { get; set; }
    public List<OrderByItem> orderByItems { get; set; } = [];
    public string? rowLimitingClause { get; set; }
    public string? error { get; set; }
}