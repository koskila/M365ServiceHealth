﻿namespace ServiceHealthReader.Services
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
            TenantId = configuration["tenantId"];
            ClientId = configuration["clientId"];
            ClientSecret = configuration["clientSecret"];

            if (ClientId == null) throw new ArgumentNullException(nameof(ClientId));
            if (ClientSecret == null) throw new ArgumentNullException(nameof(ClientSecret));
            if (TenantId == null) throw new ArgumentNullException(nameof(TenantId)); // the default value has to be there but can be overwritten
        }
    }
}
