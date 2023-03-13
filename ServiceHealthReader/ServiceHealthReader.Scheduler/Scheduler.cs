using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Diagnostics.Metrics;

namespace ServiceHealthReader.Scheduler
{
    public static class Scheduler
    {
        [FunctionName("Scheduler")]
        public static async Task Run(
            [TimerTrigger("0 */5 * * * *")] TimerInfo myTimer, ILogger log)
        {
            log.LogInformation("Scheduled ping at " + DateTime.Now);

            try
            {
                var baseUrl = Environment.GetEnvironmentVariable("BaseUrl");

                var client = new System.Net.Http.HttpClient();
                var response = await client.GetAsync(baseUrl + "/api/Announcements");

                log.LogInformation("Status: " + response.StatusCode);
                log.LogInformation("Response: " + response.Content.ReadAsStringAsync().Result);
            }
            catch (Exception ex)
            {
                log.LogWarning("Failed! ");
                log.LogError(ex, ex.Message);
            }
            finally
            {
                log.LogInformation("-----------------------------------");
            }

            return;
        }
    }
}
