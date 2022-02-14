using Timesheet.Domain.Models;

namespace Timesheet.Domain.Interfaces
{
    public interface ITimesheetService
    {
        public bool TrackTime(TimeLog timeLog);
    }
}
