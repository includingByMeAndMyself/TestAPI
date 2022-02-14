using System;
using System.Collections.Generic;
using NUnit.Framework;
using Timesheet.API.Models;
using Timesheet.API.Services;

namespace Timesheet.Tests
{
    public class ReportServiceTest
    {
        [Test]
        public void GetEmployeeReport_ShouldReturnReport()
        {
            //arrange

            var service = new ReportService();

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
