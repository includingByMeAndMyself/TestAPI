using Timesheet.Domain.Models;

namespace Timesheet.Domain.Interfaces.IRepository
{
    public interface IEmployeeRepository
    {
        StaffEmployee GetEmployee(string lastName);
        void AddEmployee(StaffEmployee staffEmployee);
    }
}
