using Microsoft.AspNetCore.Mvc;
using Timesheet.Domain.Interfaces.IService;
using Timesheet.Domain.Models;

namespace Timesheet.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StaffEmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public StaffEmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpPost]
        public ActionResult<bool> Add(StaffEmployee staffEmployee)
        {
            return Ok(_employeeService.AddEmployee(staffEmployee));
        }
    }
}
