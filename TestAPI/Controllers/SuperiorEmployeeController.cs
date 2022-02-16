using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Timesheet.Domain.Interfaces.IService;
using Timesheet.Domain.Models;

namespace Timesheet.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuperiorEmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        public SuperiorEmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpPost]
        public ActionResult<bool> Add(SuperiorEmployee SuperiorEmployee)
        {
            return Ok(_employeeService.AddEmployee(SuperiorEmployee));
        }
    }
}
