using System;
using NUnit.Framework;
using Timesheet.Application.Services;
using Timesheet.Domain.Models;

namespace Timesheet.Tests
{
    public class TimesheetServiceTest
    {
        [Test]
        public void TrackTime_ShouldReturnTrue()
        {
            //arrange

            var expectedLastName = "TestUser";
            UserSession.Sessions.Add(expectedLastName);

            var timeLog = new TimeLog()
            {
                Date = new DateTime(),
                WorkingHours = 1,
                LastName = expectedLastName,
                Comment = ""
            };

            var service = new TimesheetService();

            //act

            var result = service.TrackTime(timeLog);

            //assert

            Assert.IsTrue(result);
        }

        [TestCase(-1)]
        [TestCase(25)]
        public void TrackTime_ShouldReturnFalse(int hours)
        {
            //arrange

            var timeLog = new TimeLog()
            {
                Date = new DateTime(),
                WorkingHours = hours,
                LastName = "",
                Comment = Guid.NewGuid().ToString()
            };

            var service = new TimesheetService();

            //act

            var result = service.TrackTime(timeLog);

            //assert

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

            var service = new TimesheetService();

            //act

            var result = service.TrackTime(timeLog);

            //assert

            Assert.IsFalse(result);
        }
    }
}
