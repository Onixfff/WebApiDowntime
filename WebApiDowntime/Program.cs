
using Microsoft.EntityFrameworkCore;
using WebApiDowntime.Context;
using WebApiDowntime.Controllers;

namespace WebApiDowntime
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<dbContext>(options =>
                            options.UseMySql(builder.Configuration.GetConnectionString("Server"),
                                ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("Server"))));

            // Добавляем авторизацию
            builder.Services.AddAuthorization();

            // Добавляем контроллеры для работы с API
            builder.Services.AddControllers();  // Это необходимо для того, чтобы контроллеры работали корректно

            // Настройка Swagger для OpenAPI
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Регистрация контроллеров через DI
            builder.Services.AddScoped<DownTimeController>();

            builder.WebHost.ConfigureKestrel(options =>
            {
                options.ListenAnyIP(5048, listenOptions =>
                {
                    listenOptions.UseHttps("C:\\Users\\server\\Source\\Repos\\Onixfff\\WebApiDowntime\\WebApiDowntime\\Certificat\\certificate.pfx", "12345");
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

            app.Run();
        }
    }
}
