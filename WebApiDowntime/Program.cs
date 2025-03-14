using Microsoft.EntityFrameworkCore;
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

            builder.Services.AddDbContext<dbContext>(options =>
                            options.UseMySql(builder.Configuration.GetConnectionString("Server"),
                                ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("Server"))));

            builder.Services.AddDbContext<MacaddressregistryContext>(options =>
                            options.UseMySql(builder.Configuration.GetConnectionString("Master"),
                                ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("Master"))));

            // ��������� �����������
            builder.Services.AddAuthorization();

            // ��������� ����������� ��� ������ � API
            builder.Services.AddControllers();  // ��� ���������� ��� ����, ����� ����������� �������� ���������

            // ��������� Swagger ��� OpenAPI
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // ����������� ������������ ����� DI
            builder.Services.AddScoped<DownTimeController>();
            builder.Services.AddScoped<PLCPRU>();
            builder.Services.AddScoped<MacaddresstablesController>();

            builder.WebHost.ConfigureKestrel(options =>
            {
                options.ListenAnyIP(5048, listenOptions =>
                {
                    listenOptions.UseHttps("C:\\Users\\server\\Source\\Repos\\Onixfff\\WebApiDowntime\\WebApiDowntime\\Certificat\\certificate.pfx", "12345");
                });
            });

            var app = builder.Build();

            // ��������� ��������� HTTP-��������
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();

            // �������� ��� ������������ API
            app.MapControllers(); // ��� ����� ��� ����, ����� ���� ����������� ��������

            app.Run();
        }
    }
}
