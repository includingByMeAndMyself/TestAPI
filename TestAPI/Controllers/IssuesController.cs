using Microsoft.AspNetCore.Mvc;
using Timesheet.Domain.Interfaces.IService;

namespace Timesheet.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IssuesController : Controller
    {
        public IssuesController(IIssuesService issuesService)
        {

        }
        
        [HttpGet]
        public ActionResult Get()
        {
            

            return Ok();
        }
    }
}
