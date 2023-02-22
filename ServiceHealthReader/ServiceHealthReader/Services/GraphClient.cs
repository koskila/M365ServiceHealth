using Azure.Identity;
using Microsoft.Graph;
using ServiceHealthReader.Data.Models;
using ServiceHealthReader.Models;
using System.Text.Json;

namespace ServiceHealthReader.Services
{
    public class GraphClient
    {
        private GraphServiceClient _client;

        public GraphClient(IConfiguration configuration, IServiceConfiguration sc)
        {
            // The client credentials flow requires that you request the
            // /.default scope, and preconfigure your permissions on the
            // app registration in Azure. An administrator must grant consent
            // to those permissions beforehand.
            var scopes = new[] { "https://graph.microsoft.com/.default" };

            // Multi-tenant apps can use "common",
            // single-tenant apps must use the tenant ID from the Azure portal
            //var tenantId = "common";
            var tenantId = sc.TenantId;
            var clientId = sc.ClientId;
            var clientSecret = sc.ClientSecret;

            // using Azure.Identity;
            var options = new TokenCredentialOptions
            {
                AuthorityHost = AzureAuthorityHosts.AzurePublicCloud
            };

            // https://learn.microsoft.com/dotnet/api/azure.identity.clientsecretcredential
            var clientSecretCredential = new ClientSecretCredential(tenantId, clientId, clientSecret, options);

            // create a new httpclient using clientsecretcredential abd scopes
            // https://docs.microsoft.com/en-us/graph/api/resources/servicehealth?view=graph-rest-1.0

            _client = new GraphServiceClient(clientSecretCredential, scopes);
        }

        public IEnumerable<ServiceHealthIssue> GetIssues()
        {
            var result = _client.Admin.ServiceAnnouncement.Issues.Request()
                //.Top(5)
                //.Filter("(from/emailAddress/address) eq 'someone@someplace.com'")
                .OrderBy("lastModifiedDateTime desc")
                .GetAsync().Result;

            var list = result?.ToList();

            while (result?.NextPageRequest != null)
            {
                result = result.NextPageRequest?.GetAsync().Result;
                list.AddRange(result.ToList());
            }

            return list;
        }

        public async Task<DiagnosticResponse> GetDiagnosticInformation()
        {
            var response = await _client.Admin.ServiceAnnouncement.HealthOverviews.Request().GetResponseAsync();
            var diagnostics = response.HttpHeaders.GetValues("x-ms-ags-diagnostic");

            return JsonSerializer.Deserialize<DiagnosticResponse>(diagnostics.First());
        }
    }
}
