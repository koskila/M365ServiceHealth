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
            var issues = _applicationDbContext.Issues.OrderByDescending(X => X.Id)
                .Include(x => x.ServiceHealthIssue)
                    .ThenInclude(x => x.Posts)
                .Include(x => x.TenantIssues)
                    //.ThenInclude(x => x.TenantId)
                    .ThenInclude(x => x.Tenant)
                    .ThenInclude(x => x.ServerInfo)
            ;

            var arr = await issues.ToArrayAsync();

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
            var issue = _applicationDbContext.Issues.Where(x => x.Id == id)
                .Include(x => x.ServiceHealthIssue)
                    .ThenInclude(x => x.Posts)
                .Include(x => x.TenantIssues)
                    .ThenInclude(x => x.Tenant)
                    .ThenInclude(x => x.ServerInfo)
                .First();

            issue.TenantIssues = null; // we don't want these

            // serialize issues to JSON using System.Text.Json
            JsonSerializerOptions options = new()
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                WriteIndented = true,
            };

            var json = System.Text.Json.JsonSerializer.Serialize(issue, options);
            return json;
        }
    }
}
