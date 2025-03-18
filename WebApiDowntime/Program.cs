using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;
using WebApiDowntime.Context;
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
            builder.Services.AddScoped<PLCPRU>();
            builder.Services.AddScoped<MacaddresstablesController>();

            // Настройка Kestrel из конфигурации
            builder.WebHost.ConfigureKestrel(options =>
            {
                options.ConfigureHttpsDefaults(httpsOptions =>
                {
                    var certPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                        builder.Configuration["Kestrel:Endpoints:Https:Certificate:Path"]);
                    var certPassword = builder.Configuration["Kestrel:Endpoints:Https:Certificate:Password"];

                    if (!File.Exists(certPath))
                    {
                        throw new FileNotFoundException($"Сертификат не найден по пути: {certPath}");
                    }

                    try
                    {
                        var certBytes = File.ReadAllBytes(certPath);
                        var cert = new X509Certificate2(certBytes, certPassword,
                            X509KeyStorageFlags.MachineKeySet |
                            X509KeyStorageFlags.PersistKeySet |
                            X509KeyStorageFlags.Exportable);

                        httpsOptions.ServerCertificate = cert;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Ошибка при загрузке сертификата: {ex.Message}", ex);
                    }
                });
            });

            var app = builder.Build();

            // Настройка пайплайна HTTP-запросов
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
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
