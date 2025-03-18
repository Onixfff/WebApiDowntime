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

            // ��������� ��������� Windows Service
            builder.Host.UseWindowsService(options =>
            {
                options.ServiceName = "WebApiDowntime";
            });

            // ��������� ����������� � Windows Event Log
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

            // ��������� Kestrel �� ������������
            builder.WebHost.ConfigureKestrel(options =>
            {
                options.ConfigureHttpsDefaults(httpsOptions =>
                {
                    var certPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                        builder.Configuration["Kestrel:Endpoints:Https:Certificate:Path"]);
                    var certPassword = builder.Configuration["Kestrel:Endpoints:Https:Certificate:Password"];

                    if (!File.Exists(certPath))
                    {
                        throw new FileNotFoundException($"���������� �� ������ �� ����: {certPath}");
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
                        throw new Exception($"������ ��� �������� �����������: {ex.Message}", ex);
                    }
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

            // �������� ������ ����������
            var logger = app.Services.GetRequiredService<ILogger<Program>>();
            logger.LogInformation("WebApiDowntime Service �������");

            app.Run();
        }
    }
}
