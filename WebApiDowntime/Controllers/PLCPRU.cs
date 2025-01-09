using CSharpFunctionalExtensions;
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
        private string? _errorMessage;

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
                        throw new PlcException(ErrorCode.ConnectionError, "Не удалось подключиться к PLC.");
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

        [HttpGet("ChangeDatePRU")]
        public async Task<(bool isComplite, string? error)> ChangeDatePRU(string ipAddress, int dbNumber, int addresses, int mas, CancellationToken cancellationToken)
        {
            Result<AdressDto> resultAdresDto = await ReadValuesFromPLCInAdressAsync(ipAddress, dbNumber, addresses, cancellationToken);

            if(resultAdresDto.IsSuccess)
            {
                Result result = await ChangeValueFromPLCAdressAsync(ipAddress, dbNumber, addresses, mas, cancellationToken);

                if (result.IsSuccess)
                {
                    return (true, null);
                }
                else 
                {
                    return (false, result.Error);
                }
            }
            return (false, resultAdresDto.Error);
        }

        public async Task<(bool isComplite, string? error)> ZeroPlc(string ipAddress, int dbNumber, int addresses, CancellationToken cancellationToken)
        {
            int ZeroMas = 0;
            var isComplite = await ChangeValueFromPLCAdressAsync(ipAddress, dbNumber, addresses, ZeroMas, cancellationToken);

            if (isComplite.IsFailure)
            {
                return (false, isComplite.Error);
            }

            return (true, null);
        }

        private async Task<Result<AdressDto>> ReadValuesFromPLCInAdressAsync(string ipAddress, int dbNumber, int addresses, CancellationToken cancellationToken)
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
                        throw new PlcException(ErrorCode.ConnectionError, "Не удалось подключиться к PLC.");
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
            catch (OperationCanceledException ex)
            {
                _errorMessage = $"Операция чтения из PLC была отменена.";
                _logger.LogWarning(_errorMessage);
                return Result.Failure<AdressDto>(_errorMessage);
            }
            catch (PlcException ex)
            {
                _errorMessage = $"Ошибка PLC";
                _logger.LogError(ex, _errorMessage);
                return Result.Failure<AdressDto>(_errorMessage);
            }
            catch (Exception ex)
            {
                _errorMessage = $"Неизвестная ошибка при чтении данных из PLC.";
                _logger.LogError(ex, _errorMessage);
                return Result.Failure<AdressDto>(_errorMessage);
            }

            return Result.Success<AdressDto>(adresses);
        }

        private async Task<Result> ChangeValueFromPLCAdressAsync(string ipAddress, int dbNumber, int addresses, int mas, CancellationToken cancellationToken)
        {
            try
            {
                // Создаём соединение с PLC
                using (var plc = new Plc(CpuType.S7300, ipAddress, 0, 2))
                {
                    await plc.OpenAsync(cancellationToken);

                    if (!plc.IsConnected)
                    {
                        throw new PlcException(ErrorCode.ConnectionError, "Не удалось подключиться к PLC.");
                    }

                    await plc.WriteAsync($"DB{dbNumber}.DBD{addresses}", mas, cancellationToken);
                }
            }
            catch (OperationCanceledException ex)
            {
                _errorMessage = $"Операция чтения из PLC была отменена.";
                _logger.LogWarning(ex, _errorMessage);
                return Result.Failure(_errorMessage);
            }
            catch (PlcException ex)
            {
                _errorMessage = $"Ошибка PLC.";
                _logger.LogError(ex, _errorMessage);
                return Result.Failure(_errorMessage);
            }
            catch (Exception ex)
            {
                _errorMessage = $"Неизвестная ошибка при чтении данных из PLC.";
                _logger.LogError(ex,_errorMessage);
                return Result.Failure(_errorMessage);
            }

            return Result.Success();
        }
    } 
}
