using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceHealthReader.Data;
using ServiceHealthReader.Data.Models;
using ServiceHealthReader.Services;

namespace ServiceHealthReader.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TenantController : Controller
    {
        private readonly ApplicationDbContext _db;
        
        public TenantController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet("All")]
        public async Task<ICollection<string>> GetAllTenantIds()
        {
            var tenantIds = _db.Tenants.Select(x => x.TenantId);
            return await tenantIds.ToListAsync();
        }
    }
}
