using Microsoft.AspNetCore.Mvc;
using S7.Net;

namespace WebApiDowntime.Controllers
{
    [Route("api/[controller]")]
    public class PLCPRU : ControllerBase
    {
        private readonly ILogger<PLCPRU> _logger;

        public PLCPRU(ILogger<PLCPRU> logger)
        {
            _logger = logger;
        }

        [HttpGet("GetDatePRU")]
        private async Task<List<uint>> ReadValuesFromPLCAsync(string ipAddress, int dbNumber, List<int> addresses, CancellationToken cancellationToken)
        {
            var values = new List<uint>();

            try
            {
                // Создаём соединение с PLC
                using (var plc = new Plc(CpuType.S7300, ipAddress, 0, 2))
                {
                    await plc.OpenAsync(cancellationToken);

                    if (!plc.IsConnected)
                    {
                        throw new Exception("Не удалось подключиться к PLC.");
                    }

                    // Читаем данные из указанных адресов
                    foreach (var address in addresses)
                    {
                        cancellationToken.ThrowIfCancellationRequested();

                        var result = (uint)await plc.ReadAsync($"DB{dbNumber}.DBD{address}", cancellationToken);
                        values.Add(result);
                    }

                    plc.Close();
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("Операция чтения из PLC была отменена.");
            }
            catch (PlcException ex)
            {
                _logger.LogError(ex, "Ошибка PLC");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Неизвестная ошибка при чтении данных из PLC.");
            }
            return values;
        }

    } 
}
