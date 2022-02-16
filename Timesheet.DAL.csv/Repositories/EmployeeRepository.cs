using System;
using System.Collections.Generic;
using System.IO;
using Timesheet.DAL.CSV.Infrastructure;
using Timesheet.Domain.Interfaces.IRepository;
using Timesheet.Domain.Models;

namespace Timesheet.DAL.CSV.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly string _delimeter;
        private readonly string _path;

        public EmployeeRepository(CsvSettings csvSettings)
        {
            _delimeter = csvSettings.Delimeter;
            _path = csvSettings.Path + "\\employee.csv";
        }

        public StaffEmployee GetEmployee(string lastName)
        {
            var data = File.ReadAllText(_path);
            var timeLogs = new List<TimeLog>();
            StaffEmployee staffEmployee = null;

            foreach (var dataRow in data.Split('\n', StringSplitOptions.RemoveEmptyEntries))
            {
                var dataMembers = dataRow.Split(_delimeter);

                staffEmployee = new StaffEmployee()
                {
                    LastName = dataMembers[0],
                    Salary = decimal.TryParse(dataMembers[1], out decimal salary) ? salary : 0
                };

                break;
            }

            return staffEmployee;
        }

        public void AddEmployee(StaffEmployee staffEmployee)
        {
            var dataRow = $"{staffEmployee.LastName}{_delimeter}{staffEmployee.Salary}{_delimeter}\n";

            File.AppendAllText(_path, dataRow);
        }
    }
}
