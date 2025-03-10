public class WithItem
{
    public string? alias { get; set; }
    public List<string> columnAliases { get; set; } = [];
    public Query? subQuery { get; set; }
}