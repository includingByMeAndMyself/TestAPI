using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Timesheet.API.Models;
using Timesheet.Domain.Interfaces.IService;
using Timesheet.Domain.Models;

namespace Timesheet.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TimesheetController : Controller
    {
        private readonly ITimesheetService _timesheetService;
        private readonly IMapper _mapper;
        private readonly ILogger<TimesheetController> _logger;

        public TimesheetController(ITimesheetService timesheetService, 
                                   IMapper mapper, 
                                   ILogger<TimesheetController> logger)
        {
            _timesheetService = timesheetService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost]
        public ActionResult<bool?> TrackTime(CreateTimeLogRequest request)
        {
            var lastName = (string)HttpContext.Items["LastName"];

            _logger.LogInformation($"Пользователь {lastName} фиксирует рабочее время " + JsonConvert.SerializeObject(request, Formatting.Indented));

            if (ModelState.IsValid)
            {
                var timeLog = _mapper.Map<TimeLog>(request);

                var result = _timesheetService.TrackTime(timeLog, lastName);
                
                _logger.LogInformation($"Пользователь {lastName} успешно зафиксировал время ");
                return Ok(result);
            }
            
            _logger.LogWarning("");

            return BadRequest();
        }
    }
}
