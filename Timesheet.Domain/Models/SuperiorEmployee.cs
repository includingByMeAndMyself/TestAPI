using System.Linq;

namespace Timesheet.Domain.Models
{
    public class SuperiorEmployee : Employee
    {
        public SuperiorEmployee(string lastName, decimal salary, decimal bonus) : base(lastName, salary, "Superior")
        {
            Bonus = bonus;
        }

        public decimal Bonus { get; set; }

        public override decimal CalculateBill(TimeLog[] timeLogs)
        {
            var totalHours = timeLogs.Sum(x => x.WorkingHours);
            decimal bill = 0;

            var workingHoursGroupsByDay = timeLogs
                .GroupBy(x => x.Date.ToShortDateString());

            foreach (var workingLogsPerDay in workingHoursGroupsByDay)
            {
                int dayHours = workingLogsPerDay.Sum(x => x.WorkingHours);

                if (dayHours > MAX_WORKING_HOURS_PER_DAY)
                {

                    decimal bonusPerDay = MAX_WORKING_HOURS_PER_DAY / MAX_WORKING_HOURS_PER_MONTH * Bonus;
                    bill += MAX_WORKING_HOURS_PER_DAY / MAX_WORKING_HOURS_PER_MONTH * Salary + bonusPerDay;
                }
                else
                {
                    bill += dayHours / MAX_WORKING_HOURS_PER_MONTH * Salary;
                }
            }

            return bill;
        }

        public override string GetPersonalData(string delimeter)
        {
            return $"{LastName}{delimeter}{Salary}{delimeter}Руководитель{delimeter}{Bonus}{delimeter}\n";
        }
    }
}
