using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Timesheet.API.Models;
using Timesheet.BussinessLogic.Exceptions;
using Timesheet.Domain.Interfaces.IService;

namespace Timesheet.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IOptions<JwtConfig> _jwtConfig;

        public AuthController(IAuthService authService, IOptions<JwtConfig> jwtConfig)
        {
            _authService = authService;
            _jwtConfig = jwtConfig;
        }

        [HttpPost]
        public ActionResult<string> Login([FromBody] LoginRequest request)
        {
            try
            {
                var secret = _jwtConfig.Value.Secret;

                var token = _authService.Login(request.LastName, secret);

                return Ok(token);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch(System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
