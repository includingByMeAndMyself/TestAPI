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
        #region Should return report per days without overtime

        [Test]
        public void GetEmployeeReport_Staff_ShouldReturnReportPerDaysWithoutOvertime()
        {
            //arrange
            var timesheetRepositoryMock = new Mock<ITimesheetRepository>();
            var employeeRepositoryMock = new Mock<IEmployeeRepository>();
            
            var expectedLastName = "Иванов";
            var salary = 70000;
            decimal expectedTotal = 8750; // (8+8+4)/160 * 70000
            var expectedTotalHours = 20m; 

            timesheetRepositoryMock
                .Setup(x => x.GetTimeLogs(It.Is<string>(y => y == expectedLastName)))
                .Returns(() => new[]
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
                .Returns(() => new StaffEmployee(expectedLastName, salary))
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
        public void GetEmployeeReport_Superior_ShouldReturnReportPerDaysWithoutOvertime()
        {
            //arrange
            var timesheetRepositoryMock = new Mock<ITimesheetRepository>();
            var employeeRepositoryMock = new Mock<IEmployeeRepository>();
            
            var expectedLastName = "Иванов";
            decimal salary = 70000;
            decimal bonus = 20000;
            decimal expectedTotal = 8750;// (8+8+4)/160 * 70000
            var expectedTotalHours = 20m; 

            timesheetRepositoryMock
                .Setup(x => x.GetTimeLogs(It.Is<string>(y => y == expectedLastName)))
                .Returns(() => new[]
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
                .Returns(() => new SuperiorEmployee(expectedLastName, salary, bonus))
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
        public void GetEmployeeReport_Freelancer_ShouldReturnReportPerDaysWithoutOvertime()
        {
            //arrange
            var timesheetRepositoryMock = new Mock<ITimesheetRepository>();
            var employeeRepositoryMock = new Mock<IEmployeeRepository>();
            
            var expectedLastName = "Иванов";
            decimal salary = 1000;
            decimal expectedTotal = 20000; // (8+8+4) * 1000 
            var expectedTotalHours = 20m;

            timesheetRepositoryMock
                .Setup(x => x.GetTimeLogs(It.Is<string>(y => y == expectedLastName)))
                .Returns(() => new[]
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
                .Returns(() => new FreelancerEmployee(expectedLastName, salary))
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

        #endregion

        #region Should return report per several month

        [Test]
        public void GetEmployeeReport_Staff_ShouldReturnReportPerSeveralMonth()
        {
            //arrange
            
            var timesheetRepositoryMock = new Mock<ITimesheetRepository>();
            var employeeRepositoryMock = new Mock<IEmployeeRepository>();
            
            var expectedLastName = "Петров";
            decimal salary = 60000;
            decimal expectedTotal = 105750;
            var expectedTotalHours = 281m;

            employeeRepositoryMock
                .Setup(x => x.GetEmployee(It.Is<string>(x => x == expectedLastName)))
                .Returns(() => new StaffEmployee(expectedLastName, salary))
                .Verifiable();

            timesheetRepositoryMock
                .Setup(x => x.GetTimeLogs(It.Is<string>(x => x == expectedLastName)))
                .Returns(() =>
                {
                    TimeLog[] timeLogs = new TimeLog[35];
                    DateTime dateTime = new DateTime(2020, 11, 1);
                    timeLogs[0] = new TimeLog
                    {
                        LastName = expectedLastName,
                        Comment = Guid.NewGuid().ToString(),
                        Date = dateTime,
                        WorkingHours = 9
                    };
                    for (int i = 1; i < timeLogs.Length; i++)
                    {
                        dateTime = dateTime.AddDays(1);
                        timeLogs[i] = new TimeLog
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

            Assert.IsNotNull(result);

            Assert.AreEqual(expectedLastName, result.LastName);
            Assert.IsNotNull(result.TimeLogs);
            Assert.IsNotEmpty(result.TimeLogs);

            Assert.AreEqual(expectedTotal, result.Bill);
            Assert.AreEqual(expectedTotalHours, result.TotalHours);
        }

        [Test]
        public void GetEmployeeReport_Superior_ShouldReturnReportPerSeveralMonth()
        {
            //arrange
           
            var timesheetRepositoryMock = new Mock<ITimesheetRepository>();
            var employeeRepositoryMock = new Mock<IEmployeeRepository>();

            var expectedLastName = "Петров";
            decimal salary = 60000;
            decimal expectedTotal = 106000;
            decimal bonus = 20000;
            var expectedTotalHours = 281m;

            employeeRepositoryMock
                .Setup(x => x.GetEmployee(It.Is<string>(x => x == expectedLastName)))
                .Returns(() => new SuperiorEmployee(expectedLastName, salary, bonus))
                .Verifiable();

            timesheetRepositoryMock
                .Setup(x => x.GetTimeLogs(It.Is<string>(x => x == expectedLastName)))
                .Returns(() =>
                {
                    TimeLog[] timeLogs = new TimeLog[35];
                    DateTime dateTime = new DateTime(2020, 11, 1);
                    timeLogs[0] = new TimeLog
                    {
                        LastName = expectedLastName,
                        Comment = Guid.NewGuid().ToString(),
                        Date = dateTime,
                        WorkingHours = 9
                    };
                    for (int i = 1; i < timeLogs.Length; i++)
                    {
                        dateTime = dateTime.AddDays(1);
                        timeLogs[i] = new TimeLog
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

            Assert.IsNotNull(result);

            Assert.AreEqual(expectedLastName, result.LastName);
            Assert.IsNotNull(result.TimeLogs);
            Assert.IsNotEmpty(result.TimeLogs);

            Assert.AreEqual(expectedTotal, result.Bill);
            Assert.AreEqual(expectedTotalHours, result.TotalHours);
        }

        [Test]
        public void GetEmployeeReport_Freelancer_ShouldReturnReportPerSeveralMonth()
        {
            //arrange
           
            var timesheetRepositoryMock = new Mock<ITimesheetRepository>();
            var employeeRepositoryMock = new Mock<IEmployeeRepository>();

            var expectedLastName = "Петров";
            decimal salary = 1000;
            decimal expectedTotal = 281000;
            var expectedTotalHours = 281m;

            employeeRepositoryMock
                .Setup(x => x.GetEmployee(It.Is<string>(x => x == expectedLastName)))
                .Returns(() => new FreelancerEmployee(expectedLastName, salary))
                .Verifiable();

            timesheetRepositoryMock
                .Setup(x => x.GetTimeLogs(It.Is<string>(x => x == expectedLastName)))
                .Returns(() =>
                {
                    TimeLog[] timeLogs = new TimeLog[35];
                    DateTime dateTime = new DateTime(2020, 11, 1);
                    timeLogs[0] = new TimeLog
                    {
                        LastName = expectedLastName,
                        Comment = Guid.NewGuid().ToString(),
                        Date = dateTime,
                        WorkingHours = 9
                    };
                    for (int i = 1; i < timeLogs.Length; i++)
                    {
                        dateTime = dateTime.AddDays(1);
                        timeLogs[i] = new TimeLog
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

            Assert.IsNotNull(result);

            Assert.AreEqual(expectedLastName, result.LastName);
            Assert.IsNotNull(result.TimeLogs);
            Assert.IsNotEmpty(result.TimeLogs);

            Assert.AreEqual(expectedTotal, result.Bill);
            Assert.AreEqual(expectedTotalHours, result.TotalHours);
        }

        #endregion

        // возможен bug - тесты не проходят
        #region Should return reporn without timelogs

        [Test]
        public void GetEmployeeReport_WithoutTimeLogs()
        {
            //arrange
            var timesheetRepositoryMock = new Mock<ITimesheetRepository>();
            var employeeRepositoryMock = new Mock<IEmployeeRepository>();
            var expectedTotal = 0m;
            var expectedTotalHours = 0;

            var expectedLastName = "Петров";
            var salary = 70000m;

            timesheetRepositoryMock
                .Setup(x => x.GetTimeLogs(It.Is<string>(y => y == expectedLastName)))
                .Returns(() => new TimeLog[0])
                .Verifiable();

            employeeRepositoryMock
                .Setup(x => x.GetEmployee(It.Is<string>(y => y == expectedLastName)))
                .Returns(() => new StaffEmployee(expectedLastName, salary))
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
        public void GetEmployeeReport_SuperiorEmployee_WithoutTimeLogs()
        {
            //arrange
            var timesheetRepositoryMock = new Mock<ITimesheetRepository>();
            var employeeRepositoryMock = new Mock<IEmployeeRepository>();
            var expectedTotal = 0m;
            var expectedTotalHours = 0;

            var expectedLastName = "Сидоров";
            var salary = 70000m;
            var bonus = 20000m;

            timesheetRepositoryMock
                .Setup(x => x.GetTimeLogs(It.Is<string>(y => y == expectedLastName)))
                .Returns(() => new TimeLog[0])
                .Verifiable();

            employeeRepositoryMock
                .Setup(x => x.GetEmployee(It.Is<string>(y => y == expectedLastName)))
                .Returns(() => new SuperiorEmployee(expectedLastName, salary, bonus))
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

        #endregion

        #region Should return correct time logs for one day

        [Test]
        public void GetEmployeeReport_StaffEmployee_ShouldReturnTimeLogsForOneDay()
        {
            //arrange
            var timesheetRepositoryMock = new Mock<ITimesheetRepository>();
            var employeeRepositoryMock = new Mock<IEmployeeRepository>();
            var expectedTotalHours = 8m;

            var expectedLastName = "Петров";
            var salary = 70000m;
            var expectedTotal = 3500m;

            timesheetRepositoryMock
                .Setup(x => x.GetTimeLogs(It.Is<string>(y => y == expectedLastName)))
                .Returns(() => new[]
                {
                    new TimeLog
                    {
                        LastName = expectedLastName,
                        Comment = Guid.NewGuid().ToString(),
                        Date = DateTime.Now.AddDays(-1),
                        WorkingHours = 8
                    }
                })
                .Verifiable();

            employeeRepositoryMock
                .Setup(x => x.GetEmployee(It.Is<string>(y => y == expectedLastName)))
                .Returns(() => new StaffEmployee(expectedLastName, salary))
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
        public void GetEmployeeReport_FreelancerEmployee_ShouldReturnTimeLogsForOneDay()
        {
            //arrange
            var timesheetRepositoryMock = new Mock<ITimesheetRepository>();
            var employeeRepositoryMock = new Mock<IEmployeeRepository>();
            var expectedTotalHours = 8m;

            var expectedLastName = "Сидоров";
            var salary = 1000m;
            var expectedTotal = 8000m;

            timesheetRepositoryMock
                .Setup(x => x.GetTimeLogs(It.Is<string>(y => y == expectedLastName)))
                .Returns(() => new[]
                {
                    new TimeLog
                    {
                        LastName = expectedLastName,
                        Comment = Guid.NewGuid().ToString(),
                        Date = DateTime.Now.AddDays(-1),
                        WorkingHours = 8
                    }
                })
                .Verifiable();

            employeeRepositoryMock
                .Setup(x => x.GetEmployee(It.Is<string>(y => y == expectedLastName)))
                .Returns(() => new FreelancerEmployee(expectedLastName, salary))
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
        public void GetEmployeeReport_SuperiorEmployee_ShouldReturnTimeLogsForOneDay()
        {
            //arrange
            var timesheetRepositoryMock = new Mock<ITimesheetRepository>();
            var employeeRepositoryMock = new Mock<IEmployeeRepository>();
            var expectedTotalHours = 8m;

            var expectedLastName = "Иванов";
            var salary = 70000m;
            var bonus = 20000m;
            var expectedTotal = 3500m;

            timesheetRepositoryMock
                .Setup(x => x.GetTimeLogs(It.Is<string>(y => y == expectedLastName)))
                .Returns(() => new[]
                {
                    new TimeLog
                    {
                        LastName = expectedLastName,
                        Comment = Guid.NewGuid().ToString(),
                        Date = DateTime.Now.AddDays(-1),
                        WorkingHours = 8
                    }
                })
                .Verifiable();

            employeeRepositoryMock
                .Setup(x => x.GetEmployee(It.Is<string>(y => y == expectedLastName)))
                .Returns(() => new SuperiorEmployee(expectedLastName, salary, bonus))
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

        #endregion

        #region Should return correct timeLog with overtime for one day

        [Test]
        public void GetEmployeeReport_StaffEmployee_ShouldReturnTimeLogWithOvertimeForOneDay()
        {
            //arrange
            var timesheetRepositoryMock = new Mock<ITimesheetRepository>();
            var employeeRepositoryMock = new Mock<IEmployeeRepository>();
            var expectedTotalHours = 12;
            
            var expectedLastName = "Петров";
            var salary = 70000; 
            var expectedTotal = 7000; // (8m / 160m * 70000m) + (4m / 160m * 70000m * 2)

            timesheetRepositoryMock
                .Setup(x => x.GetTimeLogs(It.Is<string>(y => y == expectedLastName)))
                .Returns(() => new[]
                {
                    new TimeLog
                    {
                        LastName = expectedLastName,
                        Comment = Guid.NewGuid().ToString(),
                        Date = DateTime.Now.AddDays(-1),
                        WorkingHours = 12
                    }
                })
                .Verifiable();

            employeeRepositoryMock
                .Setup(x => x.GetEmployee(It.Is<string>(y => y == expectedLastName)))
                .Returns(() => new StaffEmployee(expectedLastName, salary))
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
        public void GetEmployeeReport_Freelancer_ShouldReturnTimeLogWithOvertimeForOneDay()
        {
            //arrange
            var timesheetRepositoryMock = new Mock<ITimesheetRepository>();
            var employeeRepositoryMock = new Mock<IEmployeeRepository>();
            var expectedTotalHours = 12;

            var expectedLastName = "Сидоров";
            var salary = 1000; //per 1 hour
            var expectedTotal = 12000; //12 * 1000

            timesheetRepositoryMock
                .Setup(x => x.GetTimeLogs(It.Is<string>(y => y == expectedLastName)))
                .Returns(() => new[]
                {
                    new TimeLog
                    {
                        LastName = expectedLastName,
                        Comment = Guid.NewGuid().ToString(),
                        Date = DateTime.Now.AddDays(-1),
                        WorkingHours = 12
                    }
                })
                .Verifiable();

            employeeRepositoryMock
                .Setup(x => x.GetEmployee(It.Is<string>(y => y == expectedLastName)))
                .Returns(() => new FreelancerEmployee(expectedLastName, salary)
                {
                    LastName = expectedLastName,
                    Salary = salary
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
        public void GetEmployeeReport_SuperiorEmployee_ShouldReturnTimeLogWithOvertimeForOneDay()
        {
            //arrange
            var timesheetRepositoryMock = new Mock<ITimesheetRepository>();
            var employeeRepositoryMock = new Mock<IEmployeeRepository>();
            var expectedTotalHours = 12;

            var expectedLastName = "Иванов";
            var salary = 70000m;
            var bonus = 20000m;
            var expectedTotal = 4500; // 8m/ 160m * 70000m + 8/160 * 20000 (у руководителей бонус 1000 за день вне зависимости от переаботанных часов)

            timesheetRepositoryMock
                .Setup(x => x.GetTimeLogs(It.Is<string>(y => y == expectedLastName)))
                .Returns(() => new[]
                {
                    new TimeLog
                    {
                        LastName = expectedLastName,
                        Comment = Guid.NewGuid().ToString(),
                        Date = DateTime.Now.AddDays(-1),
                        WorkingHours = 12
                    }
                })
                .Verifiable();

            employeeRepositoryMock
                .Setup(x => x.GetEmployee(It.Is<string>(y => y == expectedLastName)))
                .Returns(() => new SuperiorEmployee(expectedLastName, salary, bonus)
                {
                    LastName = expectedLastName,
                    Salary = salary
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
        
        #endregion
    }
}
