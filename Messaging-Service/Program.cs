using Messaging_Service.src._02_Application.Hubs;
using Messaging_Service.src._04_Api.Extensions;
using Messaging_Service.src._04_Api.Filters;
using Messaging_Service.src._04_Api.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// 1. تنظیمات CORS (پشتیبانی از Live Server و Localhost)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        // پذیرش هر دوی آدرس های localhost و IP لوپبک
        builder.WithOrigins("http://localhost:5500", "http://127.0.0.1:5500", "http://localhost:5099")
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials(); // برای SignalR ضروری است
    });
});

// 2. اضافه کردن کنترلرها
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidateModelStateAttribute>();
});

// 3. اضافه کردن SignalR
builder.Services.AddSignalR();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// 4. ثبت سرویس‌های لایه Application و Infrastructure
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddInfrastructureServices(connectionString);
builder.Services.AddApplicationServices();

var app = builder.Build();

// ترتیب Middlewareها بسیار مهم است

// 1. اول Exception Handling (برای مدیریت خطاها در ابتدای لوله)
app.UseMiddleware<ExceptionHandlingMiddleware>();

// 2. سپس Logging
app.UseMiddleware<LoggingMiddleware>();

// 3. سپس UseCors (باید قبل از MapControllers باشد)
app.UseCors("AllowAll");

// 4. هدایت خودکار به HTTPS (اگر این خط را بگذارید، ممکن است از HTTP جلوگیری کند)
// app.UseHttpsRedirection(); 

// 5. احراز هویت
app.UseAuthorization();

// 6. فعال کردن هاب (SignalR)
app.MapHub<MessagingHub>("/chathub");

// 7. فعال کردن کنترلرها (APIها)
app.MapControllers();

app.Run();