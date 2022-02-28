using System;
using Timeheet.DAL.MSSQL;
using Timesheet.Domain.Interfaces.IRepository;
using Timesheet.Domain.Models;
using Timeheet.DAL.MSSQL.Entities;
using System.Linq;

namespace Timesheet.DAL.MSSQL.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly TimesheetContext _context;
        public EmployeeRepository(TimesheetContext context)
        {
            _context = context;
        }

        public void Add(Domain.Models.Employee employee)
        {
            var newEmployee = new Timeheet.DAL.MSSQL.Entities.Employee
            {
                LastName = employee.LastName,
                Position = employee.Position,
                Salary = employee.Salary
            };
            
            if(employee is SuperiorEmployee superior)
            {
                newEmployee.Bonus = superior.Bonus;
            }

            _context.Employees.Add(newEmployee);
            _context.SaveChanges();
        }

        public Domain.Models.Employee Get(string lastName)
        {
            var employee = _context.Employees
                .FirstOrDefault(x => x.LastName.ToLower() == lastName.ToLower());

            if(employee == null)
            {
                return null;
            }

            switch (employee.Position)
            {
                case Position.Superior:
                    var bonus = employee.Bonus ?? 0m;
                    return new SuperiorEmployee(employee.LastName, employee.Salary, bonus);
                
                case Position.Staff:
                    return new StaffEmployee(employee.LastName, employee.Salary);

                case Position.Freelancer:
                    return new FreelancerEmployee(employee.LastName, employee.Salary);

                default:
                    throw new Exception("Wrong position: " + employee.Position);
            }

        }
    }
}
