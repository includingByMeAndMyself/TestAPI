using Timesheet.Domain.Models;

namespace Timesheet.Domain.Interfaces.IService
{
    public interface IReportService
    {
        public EmployeeReport GetEmployeeReport(string lastName);
    }
}
