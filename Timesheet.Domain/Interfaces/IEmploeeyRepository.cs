using Timesheet.Domain.Models;

namespace Timesheet.Domain.Interfaces
{
    public interface IEmploeeyRepository
    {
        StaffEmployee GetEmployee(string lastName);
    }
}
