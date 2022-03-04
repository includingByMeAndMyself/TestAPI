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
            _path = csvSettings.Path + "\\employees.csv";
        }

        public void Add(Employee employee)
        {
            var dataRow = employee.GetPersonalData(_delimeter);
            File.AppendAllText(_path, dataRow);
        }

        public Employee Get(string lastName)
        {
            var data = File.ReadAllText(_path);
            var dataRows = data.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            Employee employee = null;

            foreach (var dataRow in dataRows)
            {
                if (dataRow.Contains(lastName))
                {
                    var dataMembers = dataRow.Split(_delimeter);
                    decimal salary = 0;
                    decimal.TryParse(dataMembers[1], out salary);
                    var position = dataMembers[2];
                    switch (position)
                    {
                        case "Руководитель":
                            decimal bonus = 0;
                            decimal.TryParse(dataMembers[3], out bonus);
                            employee = new SuperiorEmployee(lastName, salary, bonus);
                            break;
                        case "Штатный сотрудник":
                            employee = new StaffEmployee(lastName, salary);
                            break;
                        case "Фрилансер":
                            employee = new FreelancerEmployee(lastName, salary);
                            break;
                        default:
                            break; // Выбрасывать исключение?
                    }
                    break;
                }
            }
            return employee;
        }
    }
}
