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

namespace ApiIsolated {
    public class HttpTrigger {
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
            var issues = _applicationDbContext.Issues.Take(10).OrderByDescending(X => X.Id)
                .Include(x => x.ServiceHealthIssue)
                    //.ThenInclude(x => x.ImpactDescription)
                    //.ThenInclude(x => x.)
                .Include(x => x.Tenant)
                    //.ThenInclude(x => x.TenantId)
                    .ThenInclude(x => x.ServerInfo)
                ;

            var arr = issues.ToArray();

            // serialize issues to JSON using System.Text.Json
            JsonSerializerOptions options = new()
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                WriteIndented = true
            };
            
            var json = System.Text.Json.JsonSerializer.Serialize(arr, options);
            return json;
        }
    }
}
