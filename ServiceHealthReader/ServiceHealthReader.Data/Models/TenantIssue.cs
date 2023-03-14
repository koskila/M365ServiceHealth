using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceHealthReader.Data.Models
{
    public class TenantIssue
    {
        public Tenant Tenant { get; set; }
        public string TenantId { get; set; }
        
        public Issue Issue { get; set; }
        public int IssueId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime FirstSeen { get; set; }
    }
}