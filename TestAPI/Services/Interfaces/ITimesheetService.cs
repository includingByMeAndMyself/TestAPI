using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timesheet.API.Models;

namespace Timesheet.API.Services.Interfaces
{
    public interface ITimesheetService
    {
        public bool TrackTime(TimeLog timeLog);
    }
}
