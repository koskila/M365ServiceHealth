using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Graph;
using ServiceHealthReader.Data.Models;
using ServiceHealthReader.Models;
using ServiceHealthReader.Services;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ServiceHealthReader.Controllers;

[ApiController]
[Route("[controller]")]
public class ServiceHealthController : Controller
{

    private readonly ILogger<ServiceHealthController> _logger;
    private readonly IConfiguration _configuration;
    private readonly IDbService _db;
    private readonly IServiceConfiguration _sc;

    public ServiceHealthController(ILogger<ServiceHealthController> logger, IConfiguration configuration, IDbService db, IServiceConfiguration sc)
    {
        _logger = logger;
        _configuration = configuration;
        _db = db;
        _sc = sc;
    }

    [HttpGet("Issues")]
    //[Route("Test")]
    //[ProducesResponseType(200)]
    public async Task<int> GetAndStoreIssues(string? tenantId)
    {
        // if TenantId was supplied, we'll use it instead of the one we got from configuration
        if (!string.IsNullOrWhiteSpace(tenantId)) { _sc.TenantId = tenantId; }

        var client = new GraphClient(_configuration, _sc);
        var items = client.GetIssues();

        Tenant t;

        if (!_db.Ctx.Tenants.Any(x => x.TenantId == _sc.TenantId))
        {
            t = _db.Ctx.Tenants.Add(new Tenant()
            {
                TenantId = _sc.TenantId,
            }).Entity;

            _db.Ctx.SaveChanges();
        }
        else
        {
            t = _db.Ctx.Tenants.Single(x => x.TenantId == _sc.TenantId);
        }

        var diag = await client.GetDiagnosticInformation();

        try
        {
            var entity = _db.Ctx.ServerInfos.Add(diag.ServerInfo);
            entity.Entity.Tenant = t;

            await _db.Ctx.SaveChangesAsync();
        }
        catch (Exception)
        {

            throw;
        }

        foreach (var item in items)
        {
            if (_db.Ctx.Issues.Count(x => x.Tenant.TenantId == _sc.TenantId && x.ServiceHealthIssue.Id == item.Id) > 0)
            {
                // handle update somehow?
            }
            else
            {
                var issue = new Issue()
                {
                    ServiceHealthIssue = item,
                    Tenant = t,
                };

                _db.Ctx.Issues.Add(issue);
            }

            _db.Ctx.SaveChanges();
        }

        return items.Count();
    }

    [HttpGet("Diagnostics")]
    public async Task<string> GetDiagnostics()
    {
        var client = new GraphClient(_configuration, _sc);
        return JsonSerializer.Serialize(await client.GetDiagnosticInformation());
    }
}