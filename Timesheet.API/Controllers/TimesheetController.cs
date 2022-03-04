using Microsoft.AspNetCore.Mvc;
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

        public TimesheetController(ITimesheetService timesheetService)
        {
            _timesheetService = timesheetService;
        }

        [HttpPost]
        public ActionResult<bool?> TrackTime(CreateTimeLogRequest request)
        {
            var lastName = (string)HttpContext.Items["lastName"];

            if (ModelState.IsValid)
            {
                var timeLog = new TimeLog
                {
                    Comment = request.Comment,
                    Date = request.Date,
                    LastName = request.LastName,
                    WorkingHours = request.WorkingHours
                };

                var result = _timesheetService.TrackTime(timeLog, timeLog.LastName);
                return Ok(result);
            }

            return BadRequest();
        }
    }
}
