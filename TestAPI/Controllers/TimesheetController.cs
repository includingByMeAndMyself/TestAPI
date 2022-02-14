using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Timesheet.API.Models;
using Timesheet.API.ResourceModels;
using Timesheet.API.Services;
using Timesheet.API.Services.Interfaces;

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
        public ActionResult<bool?> TrackTime(TimeLog request)
        {
            return Ok(_timesheetService.TrackTime(request));
        }
    }
}
