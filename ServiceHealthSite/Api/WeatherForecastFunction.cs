using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using BlazorApp.Shared;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using ServiceHealthReader.Data.Models;

namespace ApiIsolated
{
    public class HttpTrigger
    {
        private readonly ILogger _logger;

        public HttpTrigger(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<HttpTrigger>();
        }

        [Function("WeatherForecast")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
        {
            var randomNumber = new Random();
            var temp = 0;

            var result = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = temp = randomNumber.Next(-20, 55),
                Summary = GetSummary(temp)
            }).ToArray();

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.WriteAsJsonAsync(result);

            return response;
        }

        [Function("Announcements")]
        public HttpResponseData GetAnnouncements([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req) {

            List<Issue> issues = new List<Issue>();
            // create 10 randomly generated Issues
            for (var i = 0; i < 5; i++) {
                issues.Add(new Issue() {
                    Id = i,
                    ServiceHealthIssue = null,
                    Tenant = null,
                });
            }

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.WriteAsJsonAsync(issues.ToArray());

            return response;
        }

        private string GetSummary(int temp)
        {
            var summary = "Mild";

            if (temp >= 32)
            {
                summary = "Hot";
            }
            else if (temp <= 16 && temp > 0)
            {
                summary = "Cold";
            }
            else if (temp <= 0)
            {
                summary = "Freezing";
            }

            return summary;
        }
    }
}
