using System;
using System.ComponentModel.DataAnnotations; 

namespace Timesheet.Domain.Models
{
    public class TimeLog
    {
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public int WorkingHours { get; set; }
        [Required]
        public string LastName { get; set; }
        public string Comment { get; set; }
    }
}
