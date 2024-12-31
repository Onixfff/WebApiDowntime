using Microsoft.AspNetCore.Mvc;
using S7.Net;
using WebApiDowntime.Models;

namespace WebApiDowntime.Controllers
{
    [ApiController]  // Этот атрибут указывает, что класс является контроллером API
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

        [HttpPost("GetDatePRU")]
        public async Task<List<AdressDto>> ReadValuesFromPLCAsync(string ipAddress, int dbNumber, List<int> addresses, CancellationToken cancellationToken)
        {
            List<AdressDto> adresses = new List<AdressDto>();

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

                        var resultAdress = Adress.Create(_loggerAdress, address, result);

                        if (result != null)
                        {

                            if (resultAdress.IsSuccess)
                            {
                                adresses.Add(resultAdress.Value.ToDto());
                            }
                            else
                            {
                                var resultAdressZeroDate = Adress.CreateZeroDate(_loggerAdress, address, resultAdress.Error);
                                adresses.Add(resultAdressZeroDate.Value.ToDto());
                            }
                        }
                        else
                        {
                            var resultAdressZeroDate = Adress.CreateZeroDate(_loggerAdress, address, resultAdress.Error);
                            adresses.Add(resultAdressZeroDate.Value.ToDto());
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

        [HttpPost("GetDatePRUInAdres")]
        public async Task<AdressDto> ReadValuesFromPLCInAdressAsync(string ipAddress, int dbNumber, int addresses, CancellationToken cancellationToken)
        {
            AdressDto adresses = new AdressDto();

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

                    // Читаем данные из указанного адреса
                    
                    UInt32? result = (uint)await plc.ReadAsync($"DB{dbNumber}.DBD{addresses}", cancellationToken);

                    var resultAdress = Adress.Create(_loggerAdress, addresses, result);

                    if (result != null)
                    {

                        if (resultAdress.IsSuccess)
                        {
                            adresses = resultAdress.Value.ToDto();
                        }
                        else
                        {
                            var resultAdressZeroDate = Adress.CreateZeroDate(_loggerAdress, addresses, resultAdress.Error);
                            adresses = resultAdressZeroDate.Value.ToDto();
                        }
                    }
                    else
                    {
                        var resultAdressZeroDate = Adress.CreateZeroDate(_loggerAdress, addresses, resultAdress.Error);
                        adresses = resultAdressZeroDate.Value.ToDto();
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
