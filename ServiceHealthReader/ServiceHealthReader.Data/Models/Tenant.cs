using System.ComponentModel.DataAnnotations;

namespace ServiceHealthReader.Data.Models
{
    public class Tenant
    {
        public IEnumerable<Serverinfo> ServerInfo { get; set; }

        // This is actually a guid coming from AAD
        [Key]
        public string TenantId { get; set; }

        public IEnumerable<Issue> Issues { get; set; }
    }
}
