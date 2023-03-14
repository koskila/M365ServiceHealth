using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Graph;
using ServiceHealthReader.Data;
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
    private readonly ApplicationDbContext _db;
    private readonly IServiceConfiguration _sc;

    public ServiceHealthController(ILogger<ServiceHealthController> logger, IConfiguration configuration, ApplicationDbContext db, IServiceConfiguration sc)
    {
        _logger = logger;
        _configuration = configuration;
        _db = db;
        _sc = sc;
    }

    [HttpGet("Issues")]
    [HttpGet("Issues/{tenantId}")]
    //[ProducesResponseType(200)]
    public async Task<int> GetAndStoreIssues(string? tenantId)
    {
        // if TenantId was supplied, we'll use it instead of the one we got from configuration
        if (!string.IsNullOrWhiteSpace(tenantId)) { _sc.TenantId = tenantId; }

        var client = new GraphClient(_configuration, _sc);
        var items = client.GetIssues();

        // even if we've fetched items for a tenant with certain guid, we might not have said tenant in the db
        Tenant currentTenant;

        if (!_db.Tenants.Any(x => x.TenantId == _sc.TenantId))
        {
            currentTenant = _db.Tenants.Add(new Tenant()
            {
                TenantId = _sc.TenantId,
            }).Entity;

            _db.SaveChanges();
        }
        else
        {
            currentTenant = _db.Tenants.Single(x => x.TenantId == _sc.TenantId);
        }

        var diag = await client.GetDiagnosticInformation();

        try
        {
            var entity = _db.ServerInfos.Add(diag.ServerInfo);
            entity.Entity.Tenant = currentTenant;

            await _db.SaveChangesAsync();
        }
        catch (Exception)
        {
            throw;
        }

        foreach (var item in items)
        {
            if (_db.Issues.Any(x => x.ServiceHealthIssue.Id == item.Id))
            {
                var issue = _db.Issues.Where(x => x.ServiceHealthIssue.Id == item.Id).Include(x => x.TenantIssues).First();

                if (issue.TenantIssues.Where(x => x.TenantId == currentTenant.TenantId).Any())
                {
                    // we already have this issue for this tenant
                    continue;
                }
                else
                {
                    issue.TenantIssues.Add(new TenantIssue() { Issue = issue, Tenant = currentTenant });
                }
            }
            else // we don't have this issue yet
            {
                var issue = new Issue()
                {
                    ServiceHealthIssue = item
                };

                var tenantIssue = new TenantIssue() { Issue = issue, Tenant = currentTenant };
                issue.TenantIssues.Add(tenantIssue);

                _db.Issues.Add(issue);
            }

            await _db.SaveChangesAsync(); // suboptimal, but we'll take it for now
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