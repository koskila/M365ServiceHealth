namespace ServiceHealthReader.Data.Models
{
    public class Issue : Microsoft.Graph.ServiceHealthIssue
    {
        public Tenant Tenant { get; set; }
    }
}
