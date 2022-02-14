using System;
using Moq;
using NUnit.Framework;
using Timesheet.Application.Services;
using Timesheet.Domain.Interfaces;
using Timesheet.Domain.Models;

namespace Timesheet.Tests
{
    public class ReportServiceTest
    {
        [Test]
        public void GetEmployeeReport_ShouldReturnReport()
        {
            //arrange

            var timesheetRepositoryMock = new Mock<ITimesheetRepository>();
            var employeeRepositoryMock = new Mock<IEmploeeyRepository>();
            var expectedLastName = "Петров";
            var testSalary = 70000;
            var expectedTotal = 8750m; //(8+8+4)/160 * 70000

            timesheetRepositoryMock
                .Setup(x => x.GetTimeLogs(It.Is<string>(y => y == expectedLastName)))
                .Returns(() => new []
                {
                    new TimeLog
                    {
                        LastName = expectedLastName,
                        Date = DateTime.Now.AddDays(-2),
                        WorkingHours = 8,
                        Comment = Guid.NewGuid().ToString()
                    },
                    new TimeLog
                    {
                        LastName = expectedLastName,
                        Date = DateTime.Now.AddDays(-1),
                        WorkingHours = 8,
                        Comment = Guid.NewGuid().ToString()
                    },
                    new TimeLog
                    {
                        LastName = expectedLastName,
                        Date = DateTime.Now,
                        WorkingHours = 4,
                        Comment = Guid.NewGuid().ToString()
                    }
                })
                .Verifiable();

            employeeRepositoryMock
                .Setup(x => x.GetEmployee(It.Is<string>(y => y == expectedLastName)))
                .Returns(() => new StaffEmployee
                {
                    LastName = expectedLastName,
                    Salary = testSalary
                })
                .Verifiable();

            var service = new ReportService(timesheetRepositoryMock.Object, employeeRepositoryMock.Object);

            //act

            var result = service.GetEmployeeReport(expectedLastName);

            //assert

            timesheetRepositoryMock.VerifyAll();

            Assert.IsNotNull(result);
            Assert.AreEqual(expectedLastName, result.LastName);
            
            Assert.IsNotEmpty(result.TimeLogs);
            Assert.IsNotNull(result.TimeLogs);

            Assert.AreEqual(expectedTotal, result.Bill);
        }
    }
}
