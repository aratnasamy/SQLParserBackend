public interface IParseSQLService
{
    List<Query> Parse(string sql);
}