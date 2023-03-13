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

        /// <summary>
        /// Internal ID, not the actual id
        /// </summary>
        [Key]
        public int Id { get; set; }
        
        public Microsoft.Graph.ServiceHealthIssue ServiceHealthIssue { get; set; }
        
        public Tenant Tenant { get; set; }
    }
}
