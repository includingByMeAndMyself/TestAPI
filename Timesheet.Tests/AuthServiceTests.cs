 using NUnit.Framework;
 using Timesheet.Application.Services;

 namespace Timesheet.Tests
{
    public class AuthServiceTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCase("Иванов")]
        [TestCase("Петров")]
        [TestCase("Сидоров")]
        public void Login_ShouldReturnTrue(string lastName)
        {
            //arrange

            var service = new AuthService();

            //act

            var result = service.Login(lastName);

            //assert

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
            var service = new AuthService();

            //act

            var result = service.Login(lastName);

            //assert
            
            Assert.IsNotEmpty(UserSession.Sessions);
            Assert.IsNotNull(UserSession.Sessions);
            Assert.IsTrue(UserSession.Sessions.Contains(lastName));
            Assert.IsTrue(result);
        }

        [TestCase("")]
        [TestCase(null)]
        [TestCase("TestUser")]
        public void Login_ShouldReturnFalse(string lastName)
        {
            //arrange
            var service = new AuthService();

            //act

            var result = service.Login(lastName);

            //assert

            Assert.IsFalse(result);
        }
    }
}