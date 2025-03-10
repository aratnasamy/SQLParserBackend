public class SourceItem
{
    public string joinType { get; set; } = "";
    public IConditionItem? joinCondition { get; set; }
    public List<string> joinOnColumns { get; set; } = [];
    public string? schema { get; set; }
    public string? table { get; set; }
    public Query? query { get; set; }
    public string? alias { get; set; }
}