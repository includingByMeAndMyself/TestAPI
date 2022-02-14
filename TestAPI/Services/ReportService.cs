using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timesheet.API.Models;

namespace Timesheet.API.Services
{
    public class ReportService
    {
        public EmployeeReport GetEmployeeReport(string lastName)
        {
            return new EmployeeReport()
            {
                LastName = lastName
            };
        }
    }
}
