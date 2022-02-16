using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timesheet.Domain.Interfaces;
using Timesheet.Domain.Models;

namespace Timesheet.Application.Services
{
    public class EmployeeServie : IEmployeeServie
    {
        private readonly IEmploeeyRepository _emploeeyRepository;

        public EmployeeServie(IEmploeeyRepository emploeeyRepository)
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
