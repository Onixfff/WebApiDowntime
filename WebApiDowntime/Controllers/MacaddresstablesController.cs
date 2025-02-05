using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using WebApiDowntime.Models.NetworkDevices;

namespace WebApiDowntime.Controllers
{
    [ApiController]  // Этот атрибут указывает, что класс является контроллером API
    [Route("api/[controller]")]
    public class MacaddresstablesController : Controller
    {
        private readonly ILogger<MacaddresstablesController> _logger;
        private string? _errorMessage;
        private MacaddressregistryContext _dbContext;

        public MacaddresstablesController(ILogger<MacaddresstablesController> logger, MacaddressregistryContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [HttpGet("GetIpAsync")]
        public async Task<(string? ip, string? error)> GetIpAsync(string serverName)
        {
            (string ip, string error) result = new (default!, default!);

            List<Macaddresstable> devices = await _dbContext.Macaddresstables.ToListAsync();

            foreach (var device in devices)
            {
                if (device.Name == serverName)
                {
                    result.ip = device.IpAdres;
                    break;
                }
            }

            if (result.ip == null)
                result.error = "don`t have this serverName";

            return result;
        }

        [HttpGet("CheckIp")]
        public Dictionary<string, string> CheckIp()
        {
            var result = Main();
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();

            foreach (var item in result)
            {
                keyValuePairs.Add(item.ip, item.macAdres);
            }

            return keyValuePairs;
        }

        static List<(string ip, string macAdres)> Main()
        {
            List<(string ip, string macAdres)> result = new List<(string, string)>();

            string baseIp = "192.168.100."; // Замените на базовый IP вашей сети
            List<string> activeIps = new List<string>();

            for (int i = 1; i < 255; i++)
            {
                string ip = baseIp + i;
                if (PingHost(ip))
                {
                    activeIps.Add(ip);
                    Console.WriteLine($"Active IP: {ip}");
                }
            }

            foreach (var ip in activeIps)
            {
                string macAddress = GetMacAddress(ip);
                result.Add((ip, macAddress));
                Console.WriteLine($"IP: {ip}, MAC: {macAddress}");
            }

            return result;
        }

        static bool PingHost(string ip)
        {
            using (Ping pinger = new Ping())
            {
                try
                {
                    PingReply reply = pinger.Send(ip, 100);
                    return reply.Status == IPStatus.Success;
                }
                catch
                {
                    return false;
                }
            }
        }

        static string GetMacAddress(string ip)
        {
            var macAddr = string.Empty;

            // Используем команду arp для получения MAC-адреса
            var startInfo = new System.Diagnostics.ProcessStartInfo("arp", "-a " + ip)
            {
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (var process = System.Diagnostics.Process.Start(startInfo))
            {
                using (var reader = process.StandardOutput)
                {
                    string result = reader.ReadToEnd();
                    string[] lines = result.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var line in lines)
                    {
                        if (line.Contains(ip))
                        {
                            var parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            if (parts.Length >= 2)
                            {
                                macAddr = parts[1];
                            }
                        }
                    }
                }
            }

            return macAddr;
        }
    }
}
