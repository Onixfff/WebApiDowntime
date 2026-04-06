using Microsoft.EntityFrameworkCore;
using WebApiDowntime.Context;
using WebApiDowntime.Context.spslogger;
using WebApiDowntime.Controllers;
using WebApiDowntime.Models.NetworkDevices;

namespace WebApiDowntime
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Добавляем поддержку Windows Service
            builder.Host.UseWindowsService(options =>
            {
                options.ServiceName = "WebApiDowntime";
            });

            // Настройка логирования в Windows Event Log
            builder.Logging.AddEventLog(options =>
            {
                options.SourceName = "WebApiDowntime";
                options.LogName = "Application";
            });

            builder.Services.AddDbContext<dbContext>(options =>
                            options.UseMySql(builder.Configuration.GetConnectionString("Server"),
                                ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("Server"))));


            builder.Services.AddDbContext<SpsloggerContext>(options =>
                            options.UseMySql(builder.Configuration.GetConnectionString("Server"),
                                ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("Server"))));

            builder.Services.AddDbContext<MacaddressregistryContext>(options =>
                            options.UseMySql(builder.Configuration.GetConnectionString("Master"),
                                ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("Master"))));

            // Добавляем авторизацию
            builder.Services.AddAuthorization();

            // Добавляем контроллеры для работы с API
            builder.Services.AddControllers();  // Это необходимо для того, чтобы контроллеры работали корректно

            // Настройка Swagger для OpenAPI
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Регистрация контроллеров через DI
            builder.Services.AddScoped<DownTimeController>();
            builder.Services.AddScoped<SpsloggerController>();
            builder.Services.AddScoped<PLCPRU>();
            builder.Services.AddScoped<MacaddresstablesController>();

            // Настройка Kestrel из конфигурации

            var app = builder.Build();


            // Настройка пайплайна HTTP-запросов
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();

            // Маршруты для контроллеров API
            app.MapControllers(); // Это нужно для того, чтобы ваши контроллеры работали

            // Логируем запуск приложения
            var logger = app.Services.GetRequiredService<ILogger<Program>>();
            
            logger.LogInformation("WebApiDowntime Service запущен");

            app.Run();
        }
    }
}
