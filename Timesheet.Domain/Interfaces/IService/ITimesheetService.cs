using Timesheet.Domain.Models;

namespace Timesheet.Domain.Interfaces.IService
{
    public interface ITimesheetService
    {
        public bool TrackTime(TimeLog timeLog, string lastName);
    }
}
