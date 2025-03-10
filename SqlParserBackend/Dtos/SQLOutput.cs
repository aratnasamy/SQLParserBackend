public class SQLOutput
{
    public List<Query> Queries { get; set; } = [];
    public SQLOutput(List<Query> queries)
    {
        Queries = queries;
    }
}