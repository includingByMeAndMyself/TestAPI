using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timesheet.API.Models
{
    public class EmployeeReport
    {
        public string LastName { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public List<TimeLog> TimeLogs { get; set; }

        public int TotalHours { get; set; }
        public decimal Bill { get; set; }
    }
}
