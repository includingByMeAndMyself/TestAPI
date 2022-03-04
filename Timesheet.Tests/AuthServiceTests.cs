using Moq;
using NUnit.Framework;
using System;
using Timesheet.BussinessLogic.Exceptions;
using Timesheet.BussinessLogic.Services;
using Timesheet.Domain.Interfaces.IRepository;
using Timesheet.Domain.Models;

namespace Timesheet.Tests
{
    public class AuthServiceTests
    {
        private Mock<IEmployeeRepository> _employeeRepositoryMock;
        private AuthService _service;

        [SetUp]
        public void Setup()
        {
            _employeeRepositoryMock = new Mock<IEmployeeRepository>();
            _service = new AuthService(_employeeRepositoryMock.Object);
        }

        [TestCase("Иванов")]
        [TestCase("Петров")]
        [TestCase("Сидоров")]
        public void Login_ShouldReturnToken(string lastName)
        {
            //arrange
            var secret = Guid.NewGuid().ToString();

            _employeeRepositoryMock
                .Setup(x => x.Get(It.Is<string>(y => y == lastName)))
                .Returns(() => new StaffEmployee(lastName, 70000))
                .Verifiable();

            //act
            var result = _service.Login(lastName, secret);

            //assert
            _employeeRepositoryMock.VerifyAll();
            Assert.False(string.IsNullOrEmpty(result));
        }

        public void Login_InvokeLoginTwiceForOneLastName_ShouldReturnTrue()
        {
            //arrange
            var secret = Guid.NewGuid().ToString();

            string lastName = "Иванов";
            _employeeRepositoryMock
                .Setup(x => x.Get(It.Is<string>(y => y == lastName)))
                .Returns(() => new StaffEmployee(lastName, 70000))
                .Verifiable();

            //act
            var token1 = _service.Login(lastName, secret);
            var token2 = _service.Login(lastName, secret);

            //assert
            _employeeRepositoryMock.VerifyAll();

            Assert.False(string.IsNullOrEmpty(token1));
            Assert.False(string.IsNullOrEmpty(token2));
            Assert.False(token1 == token2);
        }

        [TestCase(null)]
        [TestCase("")]
        public void Login_NotValidArgument_ShouldReturnThrowArgumentException(string lastName)
        {
            //arrange
            var secret = Guid.NewGuid().ToString();

            //act 
            string result = null;
            Assert.Throws<ArgumentException>(() => result = _service.Login(lastName, secret));

            //assert
            _employeeRepositoryMock.Verify(x => x.Get(lastName), Times.Never);
            Assert.IsNull(result);
        }

        [TestCase("TestUser")]
        public void Login_UserDoesntExist_ShouldReturnThrowNotFoundException(string lastName)
        {
            //arrange
            var secret = Guid.NewGuid().ToString();

            _employeeRepositoryMock
                .Setup(x => x.Get(It.Is<string>(y => y == lastName)))
                .Returns(() => null);

            //act  
            string result = null;
            Assert.Throws<NotFoundException>(() => result = _service.Login(lastName, secret));

            //assert
            _employeeRepositoryMock.Verify(x => x.Get(lastName), Times.Once);
            Assert.IsNull(result);
        }
    }
}