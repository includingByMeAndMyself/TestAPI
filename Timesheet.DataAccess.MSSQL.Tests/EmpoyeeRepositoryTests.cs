using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Timesheet.DAL.MSSQL;
using Timesheet.DAL.MSSQL.Repositories;

namespace Timesheet.DataAccess.MSSQL.Tests
{
    public class EmpoyeeRepositoryTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Get()
        {
            var contextOptions = new DbContextOptionsBuilder<TimesheetContext>()
                .UseSqlServer("Data Source=IT-003;Database=TimesheetDB;Trusted_Connection=True;")
                .Options;

            var context = new TimesheetContext(contextOptions);

            var configuration = new MapperConfiguration(x => x.AddProfile<DataAccessMappingProfile>());
            var mapper = new Mapper(configuration);

            var repository = new EmployeeRepository(context, mapper);
            var employee = repository.Get("Иванов");
        }
    }
}