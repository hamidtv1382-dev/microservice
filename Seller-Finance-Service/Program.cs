using Microsoft.EntityFrameworkCore;
using Seller_Finance_Service.src._01_Domain.Services.Interfaces;
using Seller_Finance_Service.src._03_Infrastructure.Data;
using Seller_Finance_Service.src._04_Api.Extensions;
using Seller_Finance_Service.src._04_Api.Filters;
using Seller_Finance_Service.src._04_Api.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidateModelStateAttribute>();
});

builder.Services.AddOpenApi();

// Register Custom Services
builder.Services.AddSellerFinanceServices(builder.Configuration.GetConnectionString("DefaultConnection"));

builder.Services.AddScoped<ILoggingService, Seller_Finance_Service.src._03_Infrastructure.Services.Internal.LoggingService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMiddleware<LoggingMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

// ==========================================
// THIS SECTION IS REQUIRED FOR ADD-MIGRATION
// ==========================================
// The EF Core tools look for a class that implements IDesignTimeDbContextFactory
// if they cannot automatically create the context from the host.
public class DesignTimeDbContextFactory : Microsoft.EntityFrameworkCore.Design.IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        // Build configuration to read appsettings.json
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        optionsBuilder.UseSqlServer(connectionString);

        return new AppDbContext(optionsBuilder.Options);
    }
}