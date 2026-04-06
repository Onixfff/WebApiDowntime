using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiDowntime.Context.spslogger;

namespace WebApiDowntime.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SpsloggerController : ControllerBase
    {
        private readonly ILogger<SpsloggerController> _logger;
        private readonly SpsloggerContext _dbContext;

        public SpsloggerController(ILogger<SpsloggerController> logger, SpsloggerContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        // 🔹 GET: /api/spslogger/FullData?date=2024-01-15
        [HttpGet("FullData")]
        public async Task<IActionResult> GetFullData([FromQuery] DateTime date)
        {
            var targetDate = date == default ? DateTime.Today : date.Date;

            var startDate = targetDate.AddHours(8);
            var endDate = targetDate.AddDays(1).AddHours(8);

            _logger.LogInformation("Запрос отсчета за {Date} (период: {Start} - {End})",
                targetDate, startDate, endDate);

            const string sql = @"
                    SELECT 
                        IF(TIME(mr.Timestamp) < '08:00:00', 
                           DATE_FORMAT(DATE_SUB(mr.Timestamp, INTERVAL 1 DAY), '%d %M %Y'), 
                           DATE_FORMAT(mr.Timestamp, '%d %M %Y')) AS DateFormatted,
                           
                        IF(TIME(mr.Timestamp) BETWEEN '08:00:00' AND '20:00:00', 'день', 'ночь') AS Shift,
                        
                        mr.data_52 AS Data52,
                        
                        COUNT(mr.dbid) - COALESCE((
                            SELECT IFNULL(SUM(ms.sum_er), 0)
                            FROM spslogger.error_mas ms
                            WHERE ms.recepte = mr.data_52
                              AND DATE_FORMAT(IF(TIME(ms.data_err) < '08:00:00', DATE_SUB(ms.data_err, INTERVAL 1 DAY), ms.data_err), '%d %M %Y') = 
                                  DATE_FORMAT(IF(TIME(mr.Timestamp) < '08:00:00', DATE_SUB(mr.Timestamp, INTERVAL 1 DAY), mr.Timestamp), '%d %M %Y')
                              AND IF(TIME(ms.data_err) BETWEEN '08:00:00' AND '20:00:00', 'день', 'ночь') = 
                                  IF(TIME(mr.Timestamp) BETWEEN '08:00:00' AND '20:00:00', 'день', 'ночь')
                        ), 0) AS Count1,
                        
                        ROUND((COUNT(mr.dbid) - COALESCE((
                            SELECT IFNULL(SUM(ms.sum_er), 0)
                            FROM spslogger.error_mas ms
                            WHERE ms.recepte = mr.data_52
                              AND DATE_FORMAT(IF(TIME(ms.data_err) < '08:00:00', DATE_SUB(ms.data_err, INTERVAL 1 DAY), ms.data_err), '%d %M %Y') = 
                                  DATE_FORMAT(IF(TIME(mr.Timestamp) < '08:00:00', DATE_SUB(mr.Timestamp, INTERVAL 1 DAY), mr.Timestamp), '%d %M %Y')
                              AND IF(TIME(ms.data_err) BETWEEN '08:00:00' AND '20:00:00', 'день', 'ночь') = 
                                  IF(TIME(mr.Timestamp) BETWEEN '08:00:00' AND '20:00:00', 'день', 'ночь')
                        ), 0)) * 4.32, 2) AS Mas
                        
                    FROM spslogger.mixreport mr
                    WHERE mr.Timestamp >= @StartDate 
                      AND mr.Timestamp < @EndDate
                    GROUP BY DateFormatted, Shift, mr.data_52
                    ORDER BY mr.Timestamp ASC;";

            var results = await _dbContext.Database
                .SqlQueryRaw<ReportRecord>(sql,
                    new MySqlConnector.MySqlParameter("@StartDate", startDate),
                    new MySqlConnector.MySqlParameter("@EndDate", endDate))
                .ToListAsync();

            _logger.LogInformation("Найдено {Count} записей", results.Count);

            return Ok(results);
        }
    }

    public class ReportRecord
    {
        public string DateFormatted { get; set; } // df
        public string Shift { get; set; }
        public string Data52 { get; set; }
        public int Count1 { get; set; }
        public decimal Mas { get; set; }
    }
}
