using Microsoft.EntityFrameworkCore;
using Timeheet.DAL.MSSQL.Entities;

namespace Timeheet.DAL.MSSQL
{
    public class TimesheetContext : DbContext
    {
        public TimesheetContext(DbContextOptions<TimesheetContext> options) : base(options)
        {
                
        }

        public DbSet<TimeLog> TimeLogs { get; set; }
        public DbSet<Employee> Employees {  get; set; }
    }
}
