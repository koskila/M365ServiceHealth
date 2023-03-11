using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Azure.Functions.Worker.Configuration;
using ServiceHealthReader.Data;
using Microsoft.Extensions.DependencyInjection;

namespace ApiIsolated
{
    public class Program
    {
        public static void Main()
        {
            // get the connectionstring from appsettings.json file
            var builder = new ConfigurationBuilder()
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables().Build();

            var cs = builder.GetSection("Values").GetValue<string>("DefaultConnection");

            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults()
                .ConfigureServices(x =>
                    // get the connection string from the appsettings.json file
                    x.AddDbContext<ApplicationDbContext>(options =>
                        options.UseSqlServer(cs)
                    )
                )
                .Build();

            host.Run();
        }
    }
}