using Microsoft.EntityFrameworkCore;
using ServiceHealthReader.Data;

namespace ServiceHealthReader.Services
{
    public interface IDbService
    {
        public ApplicationDbContext Ctx { get; }
    }

    public class DbService : IDbService
    {
        private ApplicationDbContext _ctx;
        public ApplicationDbContext Ctx
        {
            get
            {
                return _ctx;
            }
        }

        public DbService(ApplicationDbContext ctx)
        {

            // Instantiate a new ApplicationDbContext
            _ctx = ctx;

            _ctx.Database.MigrateAsync();
        }
    }
}
