using System;
using System.Collections.Generic;
using System.IO;
using Timesheet.Domain.Interfaces.IRepository;
using Timesheet.Domain.Models;

namespace Timesheet.DAL.CSV.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private const string DELIMETER = ";";
        private const string PATH = "..\\Timesheet.DAL.CSV\\Data\\employee.csv";

        public StaffEmployee GetEmployee(string lastName)
        {
            var data = File.ReadAllText(PATH);
            var timeLogs = new List<TimeLog>();
            StaffEmployee staffEmployee = null;

            foreach (var dataRow in data.Split('\n', StringSplitOptions.RemoveEmptyEntries))
            {
                var dataMembers = dataRow.Split(DELIMETER);

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
            var dataRow = $"{staffEmployee.LastName}{DELIMETER}{staffEmployee.Salary}{DELIMETER}\n";

            File.AppendAllText(PATH, dataRow);
        }
    }
}
