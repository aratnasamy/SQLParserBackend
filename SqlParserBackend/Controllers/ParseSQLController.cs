using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class ParseSQLController : ControllerBase
{
    private readonly IParseSQLService _p;
    private readonly ICreateTableGraphService _c;
    public ParseSQLController(IServiceProvider serviceProvider)
    {
        _p = serviceProvider.GetService<IParseSQLService>()!;
        _c = serviceProvider.GetService<ICreateTableGraphService>()!;
    }
    [HttpGet]
    public TestItem getTestItem()
    {
        return new TestItem(1, "Hello");
    }
    [HttpPost("gettree")]
    public List<Query> ParseSQL([FromBody] SQLInput sql)
    {
        return _p.Parse(sql.sql);
    }
    [HttpPost("tablegraph")]
    public Dictionary<string,TableNode> CreateTableGraph([FromBody] SQLInput sql)
    {
        return _c.QueryGraph(_p.Parse(sql.sql)[0]);
    }
}