using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Timesheet.API.Models;
using Timesheet.API.Services;

namespace Timesheet.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        [HttpPost]
        public ActionResult<bool?> Login([FromBody] LoginRequest request)
        {
            var authService = new AuthService();
            return Ok(authService.Login(request.LastName));
        }
    }
}
