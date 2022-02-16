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

        public bool AddEmployee(Employee employee)
        {
            bool isValid = !string.IsNullOrWhiteSpace(employee.LastName) && employee.Salary > 0;

            if (isValid)
            {
                _emploeeyRepository.AddEmployee(employee);
            }

            return isValid;
        }
    }
}
