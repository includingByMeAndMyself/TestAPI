using Timesheet.Domain.Models;

namespace Timesheet.Domain.Interfaces.IService
{
    public interface IIssuesService
    {
        Issue[] Get(string login, string project);
    }
}
