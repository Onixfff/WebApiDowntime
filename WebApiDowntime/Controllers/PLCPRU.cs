using Microsoft.AspNetCore.Mvc;

namespace WebApiDowntime.Controllers
{
    public class PLCPRU : ControllerBase
    {
        private readonly ILogger<PLCPRU> _logger;

        public PLCPRU(ILogger<PLCPRU> logger)
        {
            _logger = logger;
        }


    }
}
