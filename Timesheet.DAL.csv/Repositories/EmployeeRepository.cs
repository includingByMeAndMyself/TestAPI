using System;
using Timesheet.Domain.Interfaces.IRepository;
using Timesheet.Domain.Models;

namespace Timesheet.DAL.CSV.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        public StaffEmployee GetEmployee(string lastName)
        {
            throw new NotImplementedException();
        }

        public void AddEmployee(StaffEmployee staffEmployee)
        {
            throw new NotImplementedException();
        }
    }
}
