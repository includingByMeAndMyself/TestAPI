using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timesheet.API.Services
{
    public class AuthService
    {
        public AuthService()
        {
            Employees = new List<string>
            {
                "Иванов",
                "Петров",
                "Сидоров"
            };
        }

        public List<string> Employees { get; private set; }
        public bool? Login(string lastName)
        {
            if (string.IsNullOrWhiteSpace(lastName))
            {
                return false;
            }
            return Employees.Contains(lastName);
        }
    }
}
