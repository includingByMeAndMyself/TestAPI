using System.Collections.Generic;
using System.Linq;
using Timesheet.Domain.Interfaces.IRepository;
using Timesheet.Domain.Interfaces.IService;
using Timesheet.Domain.Models;

namespace Timesheet.Application.Services
{
    public class ReportService : IReportService
    {
        private const decimal MAX_WORKING_HOURS_PER_MONTH = 160m;
        private const decimal MAX_WORKING_HOURS_PER_DAY = 8m;

        private readonly ITimesheetRepository _timesheetRepository;
        private readonly IEmployeeRepository _emploeeyRepository;

        public ReportService(ITimesheetRepository timesheetRepository, IEmployeeRepository emploeeyRepository)
        {
            _timesheetRepository = timesheetRepository;
            _emploeeyRepository = emploeeyRepository;
        }

        public EmployeeReport GetEmployeeReport(string lastName)
        {
            var employee = _emploeeyRepository.GetEmployee(lastName);
            var timeLogs = _timesheetRepository.GetTimeLogs(employee.LastName);

            if (timeLogs == null || timeLogs.Length == 0)
            {
                return new EmployeeReport()
                {
                    Bill = 0,
                    TimeLogs = new List<TimeLog>(),
                    TotalHours = 0,
                    LastName = employee.LastName
                };
            }

            var totalHours = timeLogs.Sum(x => x.WorkingHours);
            var bill = 0m;

            var workingHoursGroupsByDay = timeLogs
                .GroupBy(x => x.Date.ToShortDateString());

            foreach (var workingLogsPerDay in workingHoursGroupsByDay)
            {
                var dayHours = workingLogsPerDay.Sum(x => x.WorkingHours);

                if (dayHours > MAX_WORKING_HOURS_PER_DAY)
                {
                    var overtime = dayHours - MAX_WORKING_HOURS_PER_DAY;

                    bill += MAX_WORKING_HOURS_PER_DAY / MAX_WORKING_HOURS_PER_MONTH * employee.Salary;
                    bill += overtime / MAX_WORKING_HOURS_PER_MONTH * employee.Salary * 2m;
                }
                else
                {
                    bill += dayHours / MAX_WORKING_HOURS_PER_MONTH * employee.Salary;
                }
            }

            return new EmployeeReport()
            {
                LastName = employee.LastName,
                TimeLogs = timeLogs.ToList(),
                Bill = bill,
                TotalHours = totalHours
            }; 
        }
    }
}
