using Microsoft.AspNetCore.Mvc;
using Timesheet.Domain.Interfaces.IService;
using Timesheet.Domain.Models;

namespace Timesheet.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TimesheetController : Controller
    {
        private readonly ITimesheetService _timesheetService;

        public TimesheetController(ITimesheetService timesheetService)
        {
            _timesheetService = timesheetService;
        }

        [HttpPost]
        public ActionResult<bool?> TrackTime(TimeLog timeLog)
        {
            return Ok(_timesheetService.TrackTime(timeLog));
        }
    }
}
