using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiDowntime.Context;
using WebApiDowntime.Models;

namespace WebApiDowntime.Controllers
{
    [ApiController]  // Этот атрибут указывает, что класс является контроллером API
    [Route("api/[controller]")]  // Определяем маршрут для API
    public class DownTimeController : ControllerBase
    {
        private readonly ILogger<DownTimeController> _logger;
        private readonly dbContext _context;

        public DownTimeController(ILogger<DownTimeController> logger, dbContext context)
        {
            _logger = logger;
            _context = context;
        }

        /// <summary>
        /// Возвращает все downtime в определенном периуде
        /// </summary>
        /// <param name="start">Время начала поиска</param>
        /// <param name="end">Время конца поиска</param>
        /// <returns>
        /// Вернет list<Downtime> при успехе</returns>
        // GET api/downtime?start=2024-01-01T00:00:00&end=2024-12-31T23:59:59
        [HttpGet("downtime")]  // Атрибут для маршрута GET
        public async Task<IActionResult> GetDownTimeAsync([FromQuery] DateTime start, [FromQuery] DateTime end)
        {
            //проверка на start и end

            if (start >= end)
            {
                return BadRequest("Start >= End");
            }

            // Получаем данные о простоях из базы
            var downtimes = await _context.Downtimes
                .Where(date => date.Timestamp >= start && date.Timestamp <= end)
                .Include(d => d.IdIdleNavigation) // Загрузить связь с Ididle
                .Include(d => d.ReceptNavigation) // Загрузить связь с Recepttime
                .ToListAsync();

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
