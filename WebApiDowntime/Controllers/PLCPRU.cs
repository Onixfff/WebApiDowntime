using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using S7.Net;
using WebApiDowntime.Models;

namespace WebApiDowntime.Controllers
{
    [Route("api/[controller]")]
    public class PLCPRU : ControllerBase
    {
        private readonly ILogger<PLCPRU> _logger;
        private readonly ILogger<Adress> _loggerAdress;

        public PLCPRU(ILogger<PLCPRU> logger, ILogger<Adress> loggerAdress)
        {
            _logger = logger;
            _loggerAdress = loggerAdress;
        }

        [HttpGet("GetDatePRU")]
        private async Task<List<Adress>> ReadValuesFromPLCAsync(string ipAddress, int dbNumber, List<int> addresses, CancellationToken cancellationToken)
        {
            List<Adress> adresses = new List<Adress>();

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

                        UInt32? result = (uint)await plc.ReadAsync($"DB{dbNumber}.DBD{address}", cancellationToken);

                        if (result != 0 && result != null)
                        {
                            var resultAdress = Adress.Create(_loggerAdress, address, result);

                            if (resultAdress.IsSuccess)
                            {
                                adresses.Add(resultAdress.Value);
                            }
                            else
                            {
                                var resultAdressZeroDate = Adress.CreateZeroDate(_loggerAdress, address, resultAdress.Error);
                                adresses.Add(resultAdressZeroDate.Value);
                            }
                        }

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
            return adresses;
        }

    } 
}
