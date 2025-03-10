public class TableNode
{
    public bool Alias { get; set; }
    public Dictionary<string,TableNode> Dependencies { get; set; } = [];
}