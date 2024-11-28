using CSharpFunctionalExtensions;
using WebApiDowntime.Errors;

namespace WebApiDowntime.Services
{
    public class TimeWork
    {
        public enum TimePeriod
        {
            Day,
            Night,
            Morning
        }

        private readonly ILogger _logger;
        private readonly TimeSpan _day = new TimeSpan(8, 5, 00);
        private readonly TimeSpan _night = new TimeSpan(20, 5, 00);

        public TimeWork(ILogger logger)
        {
            _logger = logger;
        }

        public Result<TimePeriod> GetTimeWork(TimeSpan time)
        {
            TimePeriod period;

            bool isInInterval = time >= _day && time < _night;
            if (isInInterval == true)
            {
                period = TimePeriod.Day;
            }
            else
            {
                isInInterval = time >= _night;

                if (isInInterval == true)
                {
                    period = TimePeriod.Night;
                }
                else
                {
                    isInInterval = time < _day;

                    if (isInInterval == true)
                    {
                        period = TimePeriod.Morning;
                    }
                    else
                    {
                        string errorMessage = "Ошибка определения интервала времени";
                        _logger.LogError(new TimeWorkException(errorMessage, "26-57"), errorMessage);
                        return Result.Failure<TimePeriod>(errorMessage);
                    }
                }
            }

            return Result.Success(period);

        }
    }
}
