using CSharpFunctionalExtensions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebApiDowntime.Models
{
    public class Adress
    {
        private ILogger<Adress> _logger;
        private readonly int? _addres = default;
        private UInt32? _value = default;
        private string? _error = default;

        private Adress()
        {

        }

        private Adress(ILogger<Adress> logger, int? addres, UInt32? value, string? error = null)
        {
            _addres = addres;
            _value = value;
            _error = error;
            _logger = logger;
        }

        public static Result<Adress> Create(ILogger<Adress> logger, int? addres, UInt32? value, string? error = null)
        {
            string errorMessage;

            if (error != null)
            {
                errorMessage = $"Ошибка ({error})";
                logger.LogError(errorMessage);
                return Result.Failure<Adress>($"{errorMessage}");
            }

            if (addres < 0 && addres == null)
            {
                errorMessage = $"Ошибка addres donot have negative date and null";
                logger.LogError(errorMessage);
                return Result.Failure<Adress>(errorMessage);
            
            }

            if (value < 0 && value == null)
            {
                errorMessage = $"Ошибка value donot have negative date and null";
                logger.LogError(errorMessage);
                return Result.Failure<Adress>(errorMessage);
            }

            Adress adress = new Adress(logger, addres, value, default);

            return Result.Success<Adress>(adress);
        }

        public static Result<Adress> CreateZeroDate(ILogger<Adress> logger, int? addres, string error = default)
        {
            Adress adress = new Adress(logger, addres, 0, error);
            return Result.Success<Adress>(adress);
        }

        public AdressDto ToDto()
        {
            return new AdressDto
            {
                Addres = _addres,
                Value = _value,
                Error = _error
            };
        }
    }

    public class AdressDto
    {
        public int? Addres { get; set; }
        public UInt32? Value { get; set; }
        public string? Error { get; set; }
    }
}
