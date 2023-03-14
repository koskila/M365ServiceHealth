
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ServiceHealthReader.Data.Models;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;

namespace ServiceHealthReader.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public ApplicationDbContext()
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Test2");
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var options = new JsonSerializerOptions(JsonSerializerDefaults.General);

            modelBuilder
                .Entity<TenantIssue>().HasKey(x => new { x.IssueId, x.TenantId });

            modelBuilder.Entity<TenantIssue>().HasOne(x => x.Tenant).WithMany(x => x.TenantIssues).HasForeignKey(x => x.TenantId);
            modelBuilder.Entity<TenantIssue>().HasOne(x => x.Issue).WithMany(x => x.TenantIssues).HasForeignKey(x => x.IssueId);

            modelBuilder.Entity<TenantIssue>().Property(x => x.FirstSeen).HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<Serverinfo>().Property(x => x.Created).HasDefaultValueSql("GETDATE()");

            modelBuilder
                .Entity<Microsoft.Graph.ServiceHealthIssue>()
                //.Ignore(x => x.AdditionalData)
                .Ignore(x => x.Posts)
                //.Ignore(x => x.Service)
                //.Ignore(x => x.FeatureGroup)
                .Ignore(x => x.Details)
                ;
            //.Property(x => x.AdditionalData)

            modelBuilder
                .Entity<Microsoft.Graph.ServiceHealthIssue>()
                .Property(x => x.AdditionalData)
                .HasColumnName("AdditionalData")
                .HasColumnType("TEXT") // sqlite BLOB type
                .HasConversion(
                    v => JsonSerializer.Serialize(v, options),
                    s => JsonSerializer.Deserialize<IDictionary<string, object>>(s, options)!,
                    ValueComparer.CreateDefault(typeof(IDictionary<string, object>), true)
                );
        }

        public DbSet<Microsoft.Graph.ServiceHealthIssue> ServiceHealthIssues { get; set; }
        public DbSet<Issue> Issues { get; set; }
        public DbSet<Tenant> Tenants { get; set; }

        /// <summary>
        /// Diagnostics coming back from Graph
        /// </summary>
        public DbSet<Serverinfo> ServerInfos { get; set; }
    }
}
