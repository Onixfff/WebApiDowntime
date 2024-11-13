
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

            builder.Services.AddDbContext<SpsloggerContext>(options =>
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
