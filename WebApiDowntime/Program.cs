
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

            // ��������� �����������
            builder.Services.AddAuthorization();

            // ��������� ����������� ��� ������ � API
            builder.Services.AddControllers();  // ��� ���������� ��� ����, ����� ����������� �������� ���������

            // ��������� Swagger ��� OpenAPI
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // ����������� ������������ ����� DI
            builder.Services.AddScoped<DownTimeController>();

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