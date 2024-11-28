using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using WebApiDowntime.Context;
using WebApiDowntime.Errors;
using WebApiDowntime.Models;
using WebApiDowntime.Services;
using static WebApiDowntime.Services.TimeWork;

namespace WebApiDowntime.Controllers
{
    [ApiController]  // Этот атрибут указывает, что класс является контроллером API
    [Route("api/[controller]")]  // Определяем маршрут для API
    public class DownTimeController : ControllerBase
    {
        private readonly ILogger<DownTimeController> _logger;
        private readonly dbContext _context;
        private readonly TimeWork _timeWork;
        private readonly TimeOnly _day = new TimeOnly(8, 5, 00);
        private readonly TimeOnly _night = new TimeOnly(20, 5, 00);

        public DownTimeController(ILogger<DownTimeController> logger, dbContext context)
        {
            _logger = logger;
            _context = context;
            _timeWork = new TimeWork(_logger);
        }

        /// <summary>
        /// Возвращает все downtime в определенном периуде
        /// </summary>
        /// <returns>
        /// Вернет ?list<Downtime></returns>
        // GET api/downtime
        [HttpGet("downtime")]  // Атрибут для маршрута GET
        public async Task<IActionResult> GetDownTimeAsync(DateTime date)
        {
            //Заменить на поставку времени.
            TimeSpan time = new TimeSpan(date.Hour, date.Minute, date.Second);
            List<Downtime> downtimes;
            Result<TimePeriod> timePeriod = _timeWork.GetTimeWork(time);

            DateOnly dateOnly;
            DateTime start;
            DateTime end;

            if (timePeriod.IsFailure)
            {
                _logger.LogError(timePeriod.Error);
                return BadRequest($"{timePeriod.Error}");
            }

            switch (timePeriod.Value)
            {
                case TimePeriod.Day:
                    dateOnly = DateOnly.FromDateTime(date);
                    start = dateOnly.ToDateTime(_day);
                    end = dateOnly.ToDateTime(_night);

                    downtimes = await _context.Downtimes
                    .Where(date => date.Timestamp >= start && date.Timestamp < end || date.IsUpdate == false)
                    .Include(d => d.IdIdleNavigation) // Загрузить связь с Ididle
                       .Include(d => d.ReceptNavigation) // Загрузить связь с Recepttime
                       .ToListAsync();

                    break;
                case TimePeriod.Night:
                    dateOnly = DateOnly.FromDateTime(date);
                    start = dateOnly.ToDateTime(_night);
                    dateOnly = dateOnly.AddDays(1);
                    end = dateOnly.ToDateTime(_day);

                    downtimes = await _context.Downtimes
                    .Where(date => date.Timestamp >= start && date.Timestamp < end || date.IsUpdate == false)
                    .Include(d => d.IdIdleNavigation) // Загрузить связь с Ididle
                       .Include(d => d.ReceptNavigation) // Загрузить связь с Recepttime
                       .ToListAsync();

                    break;
                case TimePeriod.Morning:
                    dateOnly = DateOnly.FromDateTime(date);
                    end = dateOnly.ToDateTime(_day);
                    dateOnly = dateOnly.AddDays(-1);
                    start = dateOnly.ToDateTime(_night);

                    downtimes = await _context.Downtimes
                       .Where(date => date.Timestamp >= start && date.Timestamp < end || date.IsUpdate == false)
                       .Include(d => d.IdIdleNavigation) // Загрузить связь с Ididle
                       .Include(d => d.ReceptNavigation) // Загрузить связь с Recepttime
                       .ToListAsync();

                    break;
                default:
                    string errorMessage = "Ошибка в недопустимом значении TimePeriod";
                    var error = new DownTimesException(errorMessage, "53");
                    _logger.LogError(error, errorMessage);
                    return BadRequest(error);
            }

            // Если записи найдены, возвращаем их
            if (downtimes.Any())
            {
                return Ok(downtimes);
            }

            // Если записей нет, возвращаем 404
            return NotFound("No downtimes found in the given range.");
        }

        [HttpPut("PutDowntime")]
        public async Task<IActionResult> GetDownTimeAsync([FromQuery] List<Downtime> downtimes)
        {
            // Проверяем, что список не пустой
            if (downtimes == null || !downtimes.Any())
            {
                return BadRequest("No data provided.");
            }

            // Получаем список идентификаторов из переданных данных
            var isToUpdate = downtimes.Select(c => c.Id).ToList();

            // Ищем записи в базе данных по идентификаторам
            var recordsToUpdate = await _context.Downtimes
                .Where(cr => isToUpdate.Contains(cr.Id) == false)
                .ToListAsync();

            if (!recordsToUpdate.Any())
            {
                return NotFound("No matching records found in the database.");
            }

            //TODO Исправить изменения чтобы не проходить по циклу каждый раз
            // Обновляем поле Processed
            foreach (var record in recordsToUpdate)
            {
                for (int i = 0; i < downtimes.Count; i++)
                {
                    if (downtimes[i].Id == record.Id)
                    {
                        record.IdIdle = downtimes[i].IdIdle;
                        record.Comment = downtimes[i].Comment;
                        record.Recept = downtimes[i].Recept;
                    }
                }

                record.IsUpdate = true;
            }

            // Сохраняем изменения в базе данных
            await _context.SaveChangesAsync();

            return Ok("Records updated successfully.");
        }

        [HttpGet("ComparisonResults")]
        public async Task<IActionResult> GetComparisonResultsAsync()
        {
            var result = await _context.ComparisonResults
                .Where(isComplite => isComplite.Processed == false)
                .ToListAsync();

            if (result.Any())
            {
                return Ok(result);
            }

            return NotFound("No ComparisonResults found proccessed = false");
        }

        [HttpPut("SetComparisonResults")]
        public async Task<IActionResult> SetComparisonResultsAsync([FromBody] List<SetComparisonResults> comparisons)
        {
            // Проверяем, что список не пустой
            if (comparisons == null || !comparisons.Any())
            {
                return BadRequest("No data provided.");
            }

            // Получаем список идентификаторов из переданных данных
            var isToUpdate = comparisons.Select(c => c.Id).ToList();

            // Ищем записи в базе данных по идентификаторам
            var recordsToUpdate = await _context.ComparisonResults
                .Where(cr => isToUpdate.Contains(cr.Id))
                .ToListAsync();

            if (!recordsToUpdate.Any())
            {
                return NotFound("No matching records found in the database.");
            }

            // Обновляем поле Processed
            foreach (var record in recordsToUpdate)
            {
                record.Processed = true;
            }

            // Сохраняем изменения в базе данных
            await _context.SaveChangesAsync();

            return Ok("Records updated successfully.");
        }
    }
}
