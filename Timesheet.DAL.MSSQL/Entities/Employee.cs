using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Timeheet.DAL.MSSQL.Entities
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }
        public string LastName { get; set; }
        public decimal Salary { get; set; }
        public string Position { get; set; }
        public ICollection<TimeLog> TimeLogs { get; set; }
    }
}
