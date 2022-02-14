using Timesheet.Domain.Interfaces;
using Timesheet.Domain.Models;

namespace Timesheet.Application.Services
{
    public class ReportService : IReportService
    {
        public EmployeeReport GetEmployeeReport(string lastName)
        {
            return new EmployeeReport()
            {
                LastName = lastName
            };
        }
    }
}
