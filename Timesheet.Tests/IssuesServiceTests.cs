using NUnit.Framework;
using Timesheet.BussinessLogic.Services;

namespace Timesheet.Tests
{
    public class IssuesServiceTests
    {
        [Test]
        public void Get_ShouldReturnIssues()
        {
            //arrange

            var service = new IssuesService(null, null);

            //act

            var issues = service.Get();

            //assert

            Assert.IsNotNull(issues);
            Assert.IsNotEmpty(issues);
        }
    } 
}
