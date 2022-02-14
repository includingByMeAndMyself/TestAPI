using Moq;
using NUnit.Framework;
using Timesheet.Application.Services;
using Timesheet.Domain.Interfaces;

namespace Timesheet.Tests
{
    public class ReportServiceTest
    {
        [Test]
        public void GetEmployeeReport_ShouldReturnReport()
        {
            //arrange

            var timesheetRepositoryMock = new Mock<ITimesheetRepository>();

            var service = new ReportService(timesheetRepositoryMock.Object);

            var expectedLastName = "Иванов";
            var expectedTotal = 100m;

            //act

            var result = service.GetEmployeeReport("Иванов");

            //assert

            Assert.IsNotNull(result);
            Assert.AreEqual(expectedLastName, result.LastName);
            
            Assert.IsNotEmpty(result.TimeLogs);
            Assert.IsNotNull(result.TimeLogs);

            Assert.AreEqual(expectedTotal, result.Bill);
        }
    }
}
