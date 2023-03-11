using System.Net.Http.Headers;
using ServiceHealthReader.Data;
using ServiceHealthReader.Services;

var builder = WebApplication.CreateBuilder(args);

//builder.Configuration.AddJsonFile("appsettings.json", false);
builder.Configuration.AddJsonFile("appsettings.local.json", true);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IServiceConfiguration, ServiceConfiguration>();
builder.Services.AddScoped<IDbService, DbService>();

builder.Services.AddDbContext<ApplicationDbContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();