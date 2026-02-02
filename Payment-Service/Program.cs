using Payment_Service.src._04_Api.Extensions;
using Payment_Service.src._04_Api.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Register Application Services (Infrastructure, Domain, App layers)
builder.Services.AddPaymentServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Swagger/OpenAPI removed as per request
}

// Custom Middlewares should be registered early in the pipeline
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<LoggingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();