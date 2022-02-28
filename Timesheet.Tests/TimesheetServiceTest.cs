 using System;
 using Moq;
 using NUnit.Framework;
 using Timesheet.BussinessLogic.Services;
 using Timesheet.Domain.Interfaces.IRepository;
 using Timesheet.Domain.Models;

namespace Timesheet.Tests
{
    public class TimesheetServiceTest
    {
        private readonly TimesheetService _service;
        private readonly Mock<ITimesheetRepository> _timesheetRepositoryMock;
        private readonly Mock<IEmployeeRepository> _employeeRepositoryMock;
        public TimesheetServiceTest()
        {
            _timesheetRepositoryMock = new Mock<ITimesheetRepository>();
            _employeeRepositoryMock = new Mock<IEmployeeRepository>();


            _service = new TimesheetService(_timesheetRepositoryMock.Object, _employeeRepositoryMock.Object);
        }

        [SetUp]
        public void SetUp()
        {
            UserSession.Sessions.Clear();
        }

        [Test]
        public void TrackTime_StaffEmployee_ShouldReturnTrue()
        {
            //arrange

            var expectedLastName = "TestUser";
            UserSession.Sessions.Add(expectedLastName);

            var timeLog = new TimeLog()
            {
                Date = DateTime.Now,
                WorkingHours = 1,
                LastName = expectedLastName,
                Comment = ""
            };
          
            _timesheetRepositoryMock
                .Setup(x => x.Add(timeLog))
                .Verifiable();

            _employeeRepositoryMock
                .Setup(x => x.GetEmployee(expectedLastName))
                .Returns(() => new StaffEmployee(expectedLastName, 0m))
                .Verifiable();

            //act

            var result = _service.TrackTime(timeLog, expectedLastName);

            //assert
            
            _employeeRepositoryMock.VerifyAll();
            _timesheetRepositoryMock.Verify(x => x.Add(timeLog), Times.Once);
            Assert.IsTrue(result);
        }

        [Test]
        public void TrackTime_StaffEmployeeTryAddWithWrongLastName_ShouldReturnFalse()
        {
            //arrange

            var expectedLastName = "TestUser";
            UserSession.Sessions.Add(expectedLastName);

            var timeLog = new TimeLog()
            {
                Date = DateTime.Now,
                WorkingHours = 1,
                LastName = "NotTestUser",
                Comment = ""
            };

            _timesheetRepositoryMock
                .Setup(x => x.Add(timeLog))
                .Verifiable();

            _employeeRepositoryMock
                .Setup(x => x.GetEmployee(expectedLastName))
                .Returns(() => new StaffEmployee(expectedLastName, 0m))
                .Verifiable();

            //act

            var result = _service.TrackTime(timeLog, expectedLastName);

            //assert

            _employeeRepositoryMock.VerifyAll();
            _timesheetRepositoryMock.Verify(x => x.Add(timeLog), Times.Never);
            Assert.IsFalse(result);
        }

        [TestCase(-1, "")]
        [TestCase(25, null)]
        [TestCase(25, "")]
        [TestCase(-1, null)]
        [TestCase(-1, "TestUser")]
        [TestCase(10, null)]
        [TestCase(4, "TestUser")]
        [TestCase(8, "")]
        public void TrackTime_ShouldReturnFalse(int hours, string lastName)
        {
            //arrange

            var timeLog = new TimeLog()
            {
                Date = new DateTime(),
                WorkingHours = hours,
                LastName = lastName,
                Comment = ""
            };

            _timesheetRepositoryMock
                .Setup(x => x.Add(timeLog))
                .Verifiable();

            _employeeRepositoryMock
                .Setup(x => x.GetEmployee(lastName))
                .Returns(() => null)
                .Verifiable();

            //act

            var result = _service.TrackTime(timeLog, lastName);

            //assert

            _employeeRepositoryMock.VerifyAll();
            _timesheetRepositoryMock.Verify(x => x.Add(timeLog), Times.Never());
            Assert.IsFalse(result);
        }

        [Test]
        public void TrackTime_StaffEmployeeTrackPreviousTime_ShouldReturnTrue()
        {
            //arrange

            var expectedLastName = "TestUser";
            UserSession.Sessions.Add(expectedLastName);

            var timeLog = new TimeLog()
            {
                Date = DateTime.Now.AddDays(-10),
                WorkingHours = 1,
                LastName = expectedLastName,
                Comment = ""
            };

            _timesheetRepositoryMock
                .Setup(x => x.Add(timeLog))
                .Verifiable();

            _employeeRepositoryMock
                .Setup(x => x.GetEmployee(expectedLastName))
                .Returns(() => new StaffEmployee(expectedLastName, 0m))
                .Verifiable();

            //act

            var result = _service.TrackTime(timeLog, expectedLastName);

            //assert

            _employeeRepositoryMock.VerifyAll();
            _timesheetRepositoryMock.Verify(x => x.Add(timeLog), Times.Once);
            Assert.IsTrue(result);
        }

        [Test]
        public void TrackTime_Freelancer_ShouldReturnTrue()
        {
            //arrange

            var expectedLastName = "TestUser";

            UserSession.Sessions.Add(expectedLastName);

            var timeLog = new TimeLog()
            {
                Date = DateTime.Now,
                WorkingHours = 2,
                LastName = expectedLastName,
                Comment = Guid.NewGuid().ToString()
            };

            _employeeRepositoryMock
                .Setup(x => x.GetEmployee(expectedLastName))
                .Returns(() => new FreelancerEmployee(expectedLastName, 0m))
                .Verifiable();

            //act

            var result = _service.TrackTime(timeLog, expectedLastName);

            //assert

            var lowerBoundDate = DateTime.Now.AddDays(-2);

            _employeeRepositoryMock.VerifyAll();
            _timesheetRepositoryMock.Verify(x => x.Add(timeLog), Times.Once);
            
            Assert.True(timeLog.Date > lowerBoundDate);
            Assert.True(result);
        }

        [Test]
        public void TrackTime_Freelancer_ShouldReturnFalse()
        {
            //arrange

            var expectedLastName = "TestUser";

            UserSession.Sessions.Add(expectedLastName);

            var timeLog = new TimeLog()
            {
                Date = DateTime.Now.AddDays(-3),
                WorkingHours = 2,
                LastName = expectedLastName,
                Comment = Guid.NewGuid().ToString()
            };

            _employeeRepositoryMock
                .Setup(x => x.GetEmployee(expectedLastName))
                .Returns(() => new FreelancerEmployee(expectedLastName, 0m))
                .Verifiable();

            //act

            var result = _service.TrackTime(timeLog, expectedLastName);

            //assert

            Assert.False(result);

            _employeeRepositoryMock.VerifyAll();
            _timesheetRepositoryMock.Verify(x => x.Add(timeLog), Times.Never);
        }
    }
}
