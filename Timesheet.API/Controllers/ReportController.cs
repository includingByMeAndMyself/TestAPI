using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Timesheet.API.Models;
using Timesheet.Domain.Interfaces.IService;
using Timesheet.Domain.Models;

namespace Timesheet.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportController : Controller
    {
        private readonly IReportService _reportService;
        private readonly IMapper _mapper;

        public ReportController(IReportService reportService, IMapper mapper)
        {
            _reportService = reportService;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<GetEmployeeReportResponse> Get(string lastName)
        {
            var result = _reportService.GetEmployeeReport(lastName);

            return _mapper.Map<GetEmployeeReportResponse>(result);
        }
    }
}