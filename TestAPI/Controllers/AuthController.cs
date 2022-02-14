using Microsoft.AspNetCore.Mvc;
using Timesheet.API.ResourceModels;
using Timesheet.Domain.Interfaces;

namespace Timesheet.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        public ActionResult<bool?> Login([FromBody] LoginRequest request)
        {
            return Ok(_authService.Login(request.LastName));
        }
    }
}
