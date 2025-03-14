public class TableNode
{
    public Dictionary<string,TableNode> Dependencies { get; set; } = [];
    public TableNode(Dictionary<string,TableNode> dependencies)
    {
        Dependencies = dependencies;
    }
    public TableNode(string table)
    {
        Dependencies[table] = new TableNode([]);
    }
    public TableNode() {}
}