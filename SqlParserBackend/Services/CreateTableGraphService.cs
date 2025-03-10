public class CreateTableGraphService
{
    public List<Dictionary<string,TableNode>> CreateTableGraph(List<Query> queryList)
    {
        List<Dictionary<string,TableNode>> res = [];
        foreach (Query query in queryList) {
            Dictionary<string,TableNode> queryGraph = [];
            
            res.Add(queryGraph);
        }
        return res;
    }
    private Dictionary<string,TableNode> QueryGraph(Query query)
    {
        Dictionary<string,TableNode> queryGraph = [];
        foreach (WithItem withItem in query.withItems) {
            QueryGraph(withItem.subQuery!);
        }
        return queryGraph;
    }
}