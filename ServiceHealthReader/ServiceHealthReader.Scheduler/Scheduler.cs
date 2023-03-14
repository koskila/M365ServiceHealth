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
using System.Net.Http.Json;
using System.Collections.Generic;

namespace ServiceHealthReader.Scheduler
{
    public static class Scheduler
    {
        [FunctionName("Scheduler")]
        public static async Task Run(
            [TimerTrigger("0 */5 * * * *")] TimerInfo myTimer, ILogger log)
        {
            log.LogInformation("Scheduled ping at " + DateTime.Now);

            var baseUrl = Environment.GetEnvironmentVariable("BaseUrl");
            
            try
            {
                var client = new System.Net.Http.HttpClient();
                var tenants = await client.GetFromJsonAsync<string[]>(baseUrl + "Tenant/All");

                foreach (var tenantId in tenants)
                {
                    var response = await client.GetAsync(baseUrl + "ServiceHealth/Issues/" + tenantId);

                    log.LogInformation("Status: " + response.StatusCode);
                    log.LogInformation("Response: " + response.Content.ReadAsStringAsync().Result);
                }
                
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
