using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
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

        public HttpTrigger(ILoggerFactory loggerFactory, ApplicationDbContext applicationDbContext) {
            _logger = loggerFactory.CreateLogger<HttpTrigger>();
            _applicationDbContext = applicationDbContext;
        }

        [Function("Announcements")]
        public HttpResponseData GetAnnouncements([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req) 
        {
            var issues = _applicationDbContext.Issues.Take(10).OrderByDescending(X => X.Id).Include(x => x.ServiceHealthIssue).Include(x => x.Tenant);



            //List<Issue> issues = new List<Issue>();
            //// create 10 randomly generated Issues
            //for (var i = 0; i < 5; i++) {
            //    issues.Add(new Issue() {
            //        Id = i,
            //        ServiceHealthIssue = null,
            //        Tenant = null,
            //    });
            //}

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.WriteAsJsonAsync(issues.ToArray());

            return response;
        }
    }
}
