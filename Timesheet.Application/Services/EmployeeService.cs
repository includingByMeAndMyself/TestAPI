using Timesheet.Domain.Interfaces.IRepository;
using Timesheet.Domain.Interfaces.IService;
using Timesheet.Domain.Models;

namespace Timesheet.Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _emploeeyRepository;

        public EmployeeService(IEmployeeRepository emploeeyRepository)
        {
            _emploeeyRepository = emploeeyRepository;
        }

        public bool AddEmployee(StaffEmployee staffEmployee)
        {
            bool isValid = !string.IsNullOrWhiteSpace(staffEmployee.LastName) && staffEmployee.Salary > 0;

            if (isValid)
            {
                _emploeeyRepository.AddEmployee(staffEmployee);
            }

            return isValid;
        }
    }
}
