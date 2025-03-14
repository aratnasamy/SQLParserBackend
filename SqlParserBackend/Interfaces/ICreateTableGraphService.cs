public interface ICreateTableGraphService
{
    Dictionary<string,TableNode> QueryGraph(Query query);
}