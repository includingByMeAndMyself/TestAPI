using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Timesheet.Application.Services;
using Timesheet.Domain.Interfaces;
using Timesheet.Domain.Models;

namespace Timesheet.Tests
{
    public class EmployeeServiceTest
    {
        [Test]
        [TestCase("Иванов", 20000)]
        [TestCase("Петров", 30000)]
        [TestCase("Сидоров", 40000)]
        public void Add_ShouldReturnTrue(string lastName, int salary)
        {
            //arrage

            var staffEmployee = new StaffEmployee()
            {
                LastName = lastName,
                Salary = salary
            };

            var employeeRepository = new Mock<IEmploeeyRepository>();
            employeeRepository.Setup(x => x.AddEmployee(staffEmployee)).Verifiable();

            var service = new EmployeeServie(employeeRepository.Object);

            //act

            var result = service.AddEmployee(staffEmployee);

            //assert

            employeeRepository.Verify(x => x.AddEmployee(staffEmployee), Times.Once);
            Assert.IsTrue(result);
        }

        [Test]
        [TestCase("Иванов", 0)]
        [TestCase("Петров", -30000)]
        [TestCase("", 40000)]
        [TestCase(null, 40000)]
        [TestCase("Иванов", null)]
        public void Add_ShouldReturnFalse(string lastName, int salary)
        {
            //arrage

            var staffEmployee = new StaffEmployee()
            {
                LastName = lastName,
                Salary = salary
            };

            var employeeRepository = new Mock<IEmploeeyRepository>();

            var service = new EmployeeServie(employeeRepository.Object);

            //act

            var result = service.AddEmployee(staffEmployee);

            //assert

            employeeRepository.Verify(x => x.AddEmployee(It.IsAny<StaffEmployee>()), Times.Never);
            Assert.IsFalse(result);
        }

    }
}
