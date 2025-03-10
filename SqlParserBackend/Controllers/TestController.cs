using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase
{
    [HttpGet]
    public TestItem getTestItem()
    {
        return new TestItem(1, "Hello");
    }
}