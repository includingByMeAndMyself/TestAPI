using Timesheet.Domain.Interfaces;
using Timesheet.Domain.Models;

namespace Timesheet.DAL.CSV.Repositories
{
    public class TimesheetRepository : ITimesheetRepository
    {
        public TimeLog[] GetTimeLogs(string lastName)
        {
            throw new System.NotImplementedException();
        }
    }
}
