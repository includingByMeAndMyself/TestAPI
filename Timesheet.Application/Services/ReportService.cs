using System.Collections.Generic;
using System.Linq;
using Timesheet.Domain.Interfaces;
using Timesheet.Domain.Models;

namespace Timesheet.Application.Services
{
    public class ReportService : IReportService
    {
        private const decimal MAX_WORKING_HOURS_PER_MONTH = 160m;
        private readonly ITimesheetRepository _timesheetRepository;
        private readonly IEmploeeyRepository _emploeeyRepository;

        public ReportService(ITimesheetRepository timesheetRepository, IEmploeeyRepository emploeeyRepository)
        {
            _timesheetRepository = timesheetRepository;
            _emploeeyRepository = emploeeyRepository;
        }

        public EmployeeReport GetEmployeeReport(string lastName)
        {
            var employee = _emploeeyRepository.GetEmployee(lastName);
            var timeLogs = _timesheetRepository.GetTimeLogs(employee.LastName);

            var hours = timeLogs.Sum(x => x.WorkingHours);

            var bill = (hours / MAX_WORKING_HOURS_PER_MONTH) * employee.Salary;

            return new EmployeeReport()
            {
                LastName = employee.LastName,
                TimeLogs = timeLogs.ToList(),
                Bill = bill
            }; 
        }
    }
}
