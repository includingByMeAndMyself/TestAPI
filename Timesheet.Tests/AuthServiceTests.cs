 using Moq;
 using NUnit.Framework;
 using Timesheet.Application.Services;
 using Timesheet.Domain.Interfaces.IRepository;
 using Timesheet.Domain.Models;

 namespace Timesheet.Tests
{
    public class AuthServiceTests
    {
        [TestCase("Иванов")]
        [TestCase("Петров")]
        [TestCase("Сидоров")]
        public void Login_ShouldReturnTrue(string lastName)
        {
            //arrange

            var employeeRepositoryMock = new Mock<IEmployeeRepository>();
            employeeRepositoryMock
                .Setup(x => x.GetEmployee(It.Is<string>(y => y == lastName)))
                .Returns(() => new StaffEmployee()
                {
                    LastName = lastName,
                    Salary = 70000
                })
                .Verifiable();

            var service = new AuthService(employeeRepositoryMock.Object);

            //act

            var result = service.Login(lastName);

            //assert

            employeeRepositoryMock.VerifyAll();

            Assert.IsNotEmpty(UserSession.Sessions);
            Assert.IsNotNull(UserSession.Sessions);
            Assert.IsTrue(UserSession.Sessions.Contains(lastName));
            Assert.IsTrue(result);
        }

        [Test]
        public void Login_InvokeLoginTwiseForOneLastName_ShouldReturnTrue()
        {
            //arrange
            var lastName = "Иванов";
            var employeeRepositoryMock = new Mock<IEmployeeRepository>();
            employeeRepositoryMock
                .Setup(x => x.GetEmployee(It.Is<string>(y => y == lastName)))
                .Returns(() => new StaffEmployee()
                {
                    LastName = lastName,
                    Salary = 70000
                })
                .Verifiable();

            var service = new AuthService(employeeRepositoryMock.Object);

            //act

            var result = service.Login(lastName);

            //assert

            employeeRepositoryMock.VerifyAll();

            Assert.IsNotEmpty(UserSession.Sessions);
            Assert.IsNotNull(UserSession.Sessions);
            Assert.IsTrue(UserSession.Sessions.Contains(lastName));
            Assert.IsTrue(result);
        }

        [TestCase("TestUser")]
        public void Login_UserDoesNotExist_ShouldReturnFalse(string lastName)
        {
            //arrange

            var employeeRepositoryMock = new Mock<IEmployeeRepository>();
            employeeRepositoryMock
                .Setup(x => x.GetEmployee(lastName))
                .Returns(() => null)
                .Verifiable();

            var service = new AuthService(employeeRepositoryMock.Object);

            //act
            
            var result = service.Login(lastName);

            //assert

            employeeRepositoryMock.Verify(x => x.GetEmployee(lastName), Times.Once);
            
            Assert.IsFalse(result);
            Assert.IsTrue(UserSession.Sessions.Contains(lastName) == false);
        }

        [TestCase("")]
        [TestCase(null)]
        public void Login_NotValidArgument_ShouldReturnFalse(string lastName)
        {
            //arrange

            var employeeRepositoryMock = new Mock<IEmployeeRepository>();

            var service = new AuthService(employeeRepositoryMock.Object);

            //act

            var result = service.Login(lastName);

            //assert

            employeeRepositoryMock.Verify(x => x.GetEmployee(lastName), Times.Never);

            Assert.IsFalse(result);
            Assert.IsTrue(UserSession.Sessions.Contains(lastName) == false);
        }
    }
}