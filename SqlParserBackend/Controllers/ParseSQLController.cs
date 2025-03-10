using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class ParseSQLController : ControllerBase
{
    private readonly IParseSQLService _p;
    public ParseSQLController(IServiceProvider serviceProvider)
    {
        _p = serviceProvider.GetService<IParseSQLService>()!;
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
}