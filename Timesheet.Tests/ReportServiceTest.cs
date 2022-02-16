using System;
using Moq;
using NUnit.Framework;
using Timesheet.Application.Services;
using Timesheet.Domain.Interfaces.IRepository;
using Timesheet.Domain.Models;

namespace Timesheet.Tests
{
    public class ReportServiceTest
    {
        [Test]
        public void GetEmployeeReport_ShouldReturnReportPerOneMonth()
        {
            //arrange

            var timesheetRepositoryMock = new Mock<ITimesheetRepository>();
            var employeeRepositoryMock = new Mock<IEmployeeRepository>();
            var expectedLastName = "Петров";
            var testSalary = 70000;
            var expectedTotal = 8750m; //(8+8+4)/160 * 70000
            var expectedTotalHours = 20; //(8+8+4)

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

            Assert.IsNotNull(result.TimeLogs);
            Assert.IsNotEmpty(result.TimeLogs);

            Assert.AreEqual(expectedTotal, result.Bill);
            Assert.AreEqual(expectedTotalHours, result.TotalHours );
        }


        [Test]
        public void GetEmployeeReport_ShouldReturnReportPerSeveralMonth()
        {

            //arrange

            var timesheetRepositoryMock = new Mock<ITimesheetRepository>();
            var employeeRepositoryMock = new Mock<IEmployeeRepository>();
            var expectedLastName = "Петров";
            var testSalary = 60000;
            
            //ставка за час = 60000 / 160 = 375
            // 35 * 8 * 375 + 1 * 375 * 2

            var expectedTotal = 105750m;
            var expectedTotalHours = 281;

            employeeRepositoryMock
                .Setup(x => x.GetEmployee(It.Is<string>(x => x == expectedLastName)))
                .Returns(() => new StaffEmployee()
                {
                    LastName = expectedLastName,
                    Salary = testSalary
                })
                .Verifiable();

            timesheetRepositoryMock
                .Setup(x => x.GetTimeLogs(It.Is<string>(x => x == expectedLastName)))
                .Returns(() =>
                {
                    TimeLog[] timeLogs = new TimeLog[35];
                    DateTime dateTime = new DateTime(2020, 11, 1);

                    timeLogs[0] = new TimeLog()
                    {
                        LastName = expectedLastName,
                        Comment = Guid.NewGuid().ToString(),
                        Date = dateTime,
                        WorkingHours = 9
                    };

                    for (int i = 1; i < timeLogs.Length; i++)
                    {
                        dateTime = dateTime.AddDays(1);
                        timeLogs[i] = new TimeLog()
                        {
                            LastName = expectedLastName,
                            Comment = Guid.NewGuid().ToString(),
                            Date = dateTime,
                            WorkingHours = 8
                        };
                    }
                    return timeLogs;
                })
                .Verifiable();

            var service = new ReportService(timesheetRepositoryMock.Object, employeeRepositoryMock.Object);

            //act

            var result = service.GetEmployeeReport(expectedLastName);

            //assert

            timesheetRepositoryMock.VerifyAll();

            Assert.IsNotNull(result);
            Assert.AreEqual(expectedLastName, result.LastName);

            Assert.IsNotNull(result.TimeLogs);
            Assert.IsNotEmpty(result.TimeLogs);

            Assert.AreEqual(expectedTotal, result.Bill);
            Assert.AreEqual(expectedTotalHours, result.TotalHours);
        }

        [Test]
        public void GetEmployeeReport_WithoutTimeLogs_ShouldReportPerOneMonth()
        {
            //arrange

            var timesheetRepositoryMock = new Mock<ITimesheetRepository>();
            var employeeRepositoryMock = new Mock<IEmployeeRepository>();
            var expectedLastName = "Петров";
            var testSalary = 70000;
            var expectedTotal = 0m; 
            var expectedTotalHours = 0; 

            timesheetRepositoryMock
                .Setup(x => x.GetTimeLogs(It.Is<string>(y => y == expectedLastName)))
                .Returns(() => new TimeLog []{})
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

            Assert.IsNotNull(result.TimeLogs);
            Assert.IsEmpty(result.TimeLogs);

            Assert.AreEqual(expectedTotal, result.Bill);
            Assert.AreEqual(expectedTotalHours, result.TotalHours);
        }

        [Test]
        public void GetEmployeeReport_TimeLogs_ShouldReportForOneDay()
        {
            //arrange

            var timesheetRepositoryMock = new Mock<ITimesheetRepository>();
            var employeeRepositoryMock = new Mock<IEmployeeRepository>();
            var expectedLastName = "Петров";
            var testSalary = 70000;
            var expectedTotal = 3500m;
            var expectedTotalHours = 8;

            timesheetRepositoryMock
                .Setup(x => x.GetTimeLogs(It.Is<string>(y => y == expectedLastName)))
                .Returns(() => new []
                {
                    new TimeLog
                    {
                        LastName = expectedLastName,
                        Date = DateTime.Now.AddDays(-1),
                        WorkingHours = 8,
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

            Assert.IsNotNull(result.TimeLogs);
            Assert.IsNotEmpty(result.TimeLogs);

            Assert.AreEqual(expectedTotal, result.Bill);
            Assert.AreEqual(expectedTotalHours, result.TotalHours);
        }

        [Test]
        public void GetEmployeeReport_TimeLogsWithOvertime_ShouldReportForOneDay()
        {
            //arrange

            var timesheetRepositoryMock = new Mock<ITimesheetRepository>();
            var employeeRepositoryMock = new Mock<IEmployeeRepository>();
            var expectedLastName = "Петров";
            var testSalary = 70000;
            var expectedTotal = 8m / 160m * 70000m + (4m / 160m * (70000m * 2m));
            var expectedTotalHours = 12;

            timesheetRepositoryMock
                .Setup(x => x.GetTimeLogs(It.Is<string>(y => y == expectedLastName)))
                .Returns(() => new[]
                {
                    new TimeLog
                    {
                        LastName = expectedLastName,
                        Date = DateTime.Now.AddDays(-1),
                        WorkingHours = 12,
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

            Assert.IsNotNull(result.TimeLogs);
            Assert.IsNotEmpty(result.TimeLogs);

            Assert.AreEqual(expectedTotal, result.Bill);
            Assert.AreEqual(expectedTotalHours, result.TotalHours);
        }
    }
}
