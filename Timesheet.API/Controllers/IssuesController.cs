using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Timesheet.API.Models;
using Timesheet.Domain.Interfaces.IService;

namespace Timesheet.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IssuesController : Controller
    {
        private readonly IIssuesService _issuesService;
        private readonly IMapper _mapper;

        public IssuesController(IIssuesService issuesService, IMapper mapper)
        {
            _issuesService = issuesService;
            _mapper = mapper;
        }
        
        [HttpGet]
        public ActionResult<GetIssuesResponse> Get()
        {
            var issues = _issuesService.Get("includingByMeAndMyself", "TestAPI");
            return new GetIssuesResponse
            {
                Issues = _mapper.Map<IssueDto[]>(issues)
            };
        }
    }
}
