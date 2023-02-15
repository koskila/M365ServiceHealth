using Azure.Identity;
using Microsoft.Graph;
using ServiceHealthReader.Models;

namespace ServiceHealthReader.Services
{
    public class GraphClient
    {
        private GraphServiceClient _client;

        public GraphClient(IConfiguration configuration)
        {
            // The client credentials flow requires that you request the
            // /.default scope, and preconfigure your permissions on the
            // app registration in Azure. An administrator must grant consent
            // to those permissions beforehand.
            var scopes = new[] { "https://graph.microsoft.com/.default" };

            // Multi-tenant apps can use "common",
            // single-tenant apps must use the tenant ID from the Azure portal
            //var tenantId = "common";
            var tenantId = configuration["auth:tenantId"];

            // Values from app registration, get clientid and clientsecret from appsettings.json
            var clientId = configuration["auth:clientId"];
            var clientSecret = configuration["auth:clientSecret"];

            // using Azure.Identity;
            var options = new TokenCredentialOptions
            {
                AuthorityHost = AzureAuthorityHosts.AzurePublicCloud
            };

            // https://learn.microsoft.com/dotnet/api/azure.identity.clientsecretcredential
            var clientSecretCredential = new ClientSecretCredential(
                tenantId, clientId, clientSecret, options);

            // create a new httpclient using clientsecretcredential abd scopes
            // https://docs.microsoft.com/en-us/graph/api/resources/servicehealth?view=graph-rest-1.0

            _client = new GraphServiceClient(clientSecretCredential, scopes);
        }

        public IEnumerable<ServiceHealthIssue> GetServiceHealthIssueRootObject()
        {
            var result = _client.Admin.ServiceAnnouncement.Issues.Request().GetAsync().Result;

            var list = result.ToList();

            
            var nextPAge = result.NextPageRequest.GetAsync().Result;
            while (nextPAge != null)
            {
                list.AddRange(nextPAge.ToList());
                nextPAge = nextPAge.NextPageRequest?.GetAsync().Result;
            }

            return list;
        }

        public async Task<string> GetDiagnosticInformation()
        {
            var response = await _client.Admin.ServiceAnnouncement.HealthOverviews.Request().GetResponseAsync();
            var diagnostics = response.HttpHeaders.GetValues("x-ms-ags-diagnostic");

            return diagnostics.ToString();
        }
    }
}
