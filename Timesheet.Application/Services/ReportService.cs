using System.Collections.Generic;
using System.Linq;
using Timesheet.Domain.Interfaces;
using Timesheet.Domain.Models;

namespace Timesheet.Application.Services
{
    public class ReportService : IReportService
    {
        private readonly ITimesheetRepository _timesheetRepository;

        public ReportService(ITimesheetRepository timesheetRepository)
        {
            _timesheetRepository = timesheetRepository; 
        }
        public EmployeeReport GetEmployeeReport(string lastName)
        {
            var timeLogs = _timesheetRepository.GetTimeLogs(lastName);

            return new EmployeeReport()
            {
                LastName = lastName,
                TimeLogs = timeLogs.ToList()
            };
        }
    }
}
