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
    public override string ToString()
    {
        string res = $"{queryJoin} ";
        foreach (WithItem withItem in withItems) {
            res += $"WITH {withItem.alias} AS {withItem.subQuery} ";
        }
        res += $"SELECT {selectModifier} ";
        foreach (SelectItem selectItem in selectItems) {
            if (selectItem.expression != null) {
                res += $"{selectItem.expression} ";
            }
            else {
                if (selectItem.schema != null) {
                    res += $"{selectItem.schema}.";
                }
                if (selectItem.table != null) {
                    res += $"{selectItem.table}.";
                }
                res += $"{selectItem.field} ";
            }
            if (selectItem.alias != null) {
                res += $"AS {selectItem.alias} "; 
            }
            res += ", ";
        }
        res += "FROM ";
        foreach (List<SourceItem> sourceList in sources) {
            foreach (SourceItem source in sourceList) {
                res += $"{source} ";
            }
            res += ", ";
        }
        if (whereCondition != null) {
            res += $"{whereCondition} ";
        }
        if (groupByExpressions.Count > 0) {
            res += "GROUP BY ";
            foreach (IExpressionItem expressionItem in groupByExpressions) {
                res += $"{expressionItem}, ";
            }
            if (groupByCondition != null) {
                res += $"HAVING {groupByCondition} ";
            }
        }
        if (orderByModifier != null) {
            res += $"{orderByModifier} ";
            foreach (OrderByItem orderByItem in orderByItems) {
                res += $"{orderByItem.expression} ";
                if (orderByItem.direction != null) {
                    res += $"{orderByItem.direction} ";
                }
                if (orderByItem.nulls != null) {
                    res += $"NULLS {orderByItem.nulls} ";
                }
                res += ", ";
            }
        }
        if (rowLimitingClause != null) {
            res += $"{rowLimitingClause} ";
        }

        return res;
    }
}