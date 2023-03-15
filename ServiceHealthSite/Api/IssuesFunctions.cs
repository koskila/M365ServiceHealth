using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using BlazorApp.Shared;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServiceHealthReader.Data;
using ServiceHealthReader.Data.Models;
using System.Net.Http;

namespace ApiIsolated
{
    public class HttpTrigger
    {
        private readonly ILogger _logger;
        private readonly ApplicationDbContext _applicationDbContext;

        public HttpTrigger(ILoggerFactory loggerFactory, ApplicationDbContext applicationDbContext)
        {
            _logger = loggerFactory.CreateLogger<HttpTrigger>();
            _applicationDbContext = applicationDbContext;
        }

        [Function("Announcements")]
        public async Task<string> GetAnnouncements([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
        {
            var issues = _applicationDbContext.Issues
                .Include(x => x.ServiceHealthIssue)
                    .ThenInclude(x => x.Posts)
                .Include(x => x.TenantIssues)
                    //.ThenInclude(x => x.TenantId)
                    .ThenInclude(x => x.Tenant)
                    .ThenInclude(x => x.ServerInfo)
                .AsSplitQuery()
            ;

            var arr = await issues.ToListAsync();

            // serialize issues to JSON using System.Text.Json
            JsonSerializerOptions options = new()
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                WriteIndented = true
            };

            var json = System.Text.Json.JsonSerializer.Serialize(arr, options);
            return json;
        }

        [Function("Issue")]
        // get id parameter from the query string
        public async Task<string> GetOneIssue([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Issue/{id:int}")] HttpRequestData req, int id)
        {
            var issue = await _applicationDbContext.Issues.Where(x => x.Id == id)
                .Include(x => x.ServiceHealthIssue)
                    .ThenInclude(x => x.Posts)
                .Include(x => x.TenantIssues)
                    .ThenInclude(x => x.Tenant)
                    .ThenInclude(x => x.ServerInfo)
                .AsSplitQuery()
                .FirstAsync();

            //issue.TenantIssues = null; // we don't want these

            // serialize issues to JSON using System.Text.Json
            JsonSerializerOptions options = new()
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                WriteIndented = true,
            };

            var json = System.Text.Json.JsonSerializer.Serialize(issue, options);
            return json;
        }

        [Function("TenantExists")]
        public async Task<string> TenantExists([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "TenantExists/{id}")] HttpRequestData req, string id)
        {
            if (_applicationDbContext.Tenants.Any(x => x.TenantId == id))
            {
                return System.Text.Json.JsonSerializer.Serialize("true");
            }
            else
            {
                return System.Text.Json.JsonSerializer.Serialize("false");
            }
        }

        [Function("AllGeos")]
        public async Task<List<string>> AllGeos([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "AllGeos")] HttpRequestData req)
        {
            var geos = _applicationDbContext.ServerInfos.Select(x => x.DataCenter).ToList();
            geos.Add("Global");
            return geos;
        }

        [Function("Trigger")]
        public async Task Trigger([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Trigger/{id}")] HttpRequestData req, string id)
        {
            var baseUrl = Environment.GetEnvironmentVariable("BaseUrl");

            var httpClient = new HttpClient();
            await httpClient.GetAsync(baseUrl + "ServiceHealth/Issues/" + id);
        }
    }
}