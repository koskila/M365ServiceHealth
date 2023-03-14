using System.ComponentModel.DataAnnotations;

namespace ServiceHealthReader.Data.Models
{
    public class Tenant
    {
        public ICollection<Serverinfo> ServerInfo { get; set; } = new List<Serverinfo>();

        // This is actually a guid coming from AAD
        [Key]
        public string TenantId { get; set; }

        public ICollection<TenantIssue> TenantIssues { get; set; } = new List<TenantIssue>();
    }
}
