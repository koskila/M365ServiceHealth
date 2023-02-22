namespace ServiceHealthReader.Services
{
    public interface IServiceConfiguration
    {
        string TenantId { get; set; } // TenantId can be overridden
        string ClientId { get; }
        string ClientSecret { get; }
    }
    public class ServiceConfiguration : IServiceConfiguration
    {
        public string TenantId { get; set;  }
        public string ClientId { get; }
        public string ClientSecret { get; }

        public ServiceConfiguration (IConfiguration configuration)
        {
            TenantId = configuration["auth:tenantId"];
            ClientId = configuration["auth:clientId"];
            ClientSecret = configuration["auth:clientSecret"];
        }
    }
}
