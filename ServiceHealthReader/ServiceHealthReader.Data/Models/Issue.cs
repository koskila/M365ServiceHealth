using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceHealthReader.Data.Models
{
    public class Issue
    {
        [Key]
        public int Id { get; set; }
        public Microsoft.Graph.ServiceHealthIssue ServiceHealthIssue { get; set; }
        public Tenant Tenant { get; set; }
    }
}
