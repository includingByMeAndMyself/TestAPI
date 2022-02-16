using Timesheet.Domain.Models;

namespace Timesheet.Domain.Interfaces.IRepository
{
    public interface IEmployeeRepository
    {
        Employee GetEmployee(string lastName);
        void AddEmployee(Employee employee);
    }
}
