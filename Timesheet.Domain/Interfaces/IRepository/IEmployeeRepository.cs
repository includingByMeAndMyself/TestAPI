using Timesheet.Domain.Models;

namespace Timesheet.Domain.Interfaces.IRepository
{
    public interface IEmployeeRepository
    {
        Employee Get(string lastName);
        void Add(Employee employee);
    }
}
