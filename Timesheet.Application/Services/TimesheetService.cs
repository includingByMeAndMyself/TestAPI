using Timesheet.Domain.Interfaces.IRepository;
using Timesheet.Domain.Interfaces.IService;
using Timesheet.Domain.Models;

namespace Timesheet.Application.Services
{
    public class TimesheetService : ITimesheetService
    {
        private readonly ITimesheetRepository _timesheetRepository;

        public TimesheetService(ITimesheetRepository timesheetRepository)
        {
            _timesheetRepository = timesheetRepository;
        }

        public bool TrackTime(TimeLog timeLog)
        {
            bool isValid = timeLog.WorkingHours > 0 
                           && timeLog.WorkingHours <= 24
                           && !string.IsNullOrWhiteSpace(timeLog.LastName);

            isValid = UserSession.Sessions.Contains(timeLog.LastName);

            if (isValid == false)
            {
                return false;
            }

            _timesheetRepository.Add(timeLog);

            return true;
        } 
    }
}
