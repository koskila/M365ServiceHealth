using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using ServiceHealthReader.Models;
using ServiceHealthReader.Services;

namespace ServiceHealthReader.Controllers;

[ApiController]
[Route("[controller]")]
public class ServiceHealthController : Controller
{

    private readonly ILogger<ServiceHealthController> _logger;
    private readonly IConfiguration _configuration;

    public ServiceHealthController(ILogger<ServiceHealthController> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    [HttpGet(Name = "Test")]
    //[Route("Test")]
    //[ProducesResponseType(200)]
    public int Test()
    {
        var client = new GraphClient(_configuration);
        var items = client.GetServiceHealthIssueRootObject();

        return items.Count();
    }

    [HttpGet(Name = "GetDiagnostics")]
    public async Task<string> GetDiagnostics()
    {
        var client = new GraphClient(_configuration);
        return await client.GetDiagnosticInformation();
    }
}