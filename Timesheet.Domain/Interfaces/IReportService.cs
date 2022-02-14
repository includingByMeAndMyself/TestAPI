using Timesheet.Domain.Models;

namespace Timesheet.Domain.Interfaces
{
    public interface IReportService
    {
        public EmployeeReport GetEmployeeReport(string lastName);
    }
}
