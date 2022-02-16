using System.Collections.Generic;
using Timesheet.Domain.Interfaces.IRepository;
using Timesheet.Domain.Interfaces.IService;

namespace Timesheet.Application.Services
{
    public class AuthService : IAuthService
    {
        IEmployeeRepository _employeeRepository;

        public AuthService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public bool? Login(string lastName)
        {
            if (string.IsNullOrWhiteSpace(lastName))
            {
                return false;
            }

            var Employee = _employeeRepository.GetEmployee(lastName);
            var isEmployeeExist = Employee != null;

            if (isEmployeeExist)
            {
                UserSession.Sessions.Add(lastName);
            }

            return isEmployeeExist;
        }
    }

    public static class UserSession
    {
        public static HashSet<string> Sessions { get; set; } = new HashSet<string>(); 
    }
}
