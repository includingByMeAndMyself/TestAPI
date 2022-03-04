using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Timesheet.API.Models;
using Timesheet.BussinessLogic.Exceptions;
using Timesheet.Domain.Interfaces.IService;

namespace Timesheet.API.Controllers
{
    /// <summary>
    /// Controller to work with auth service
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IOptions<JwtConfig> _jwtConfig;
        private readonly ILogger _logger;

        public AuthController(IAuthService authService, 
            IOptions<JwtConfig> jwtConfig,
            ILogger logger)
        {
            _authService = authService;
            _jwtConfig = jwtConfig;
            _logger = logger;
        }

        /// <summary>
        /// Login in timeshett api
        /// </summary>
        /// <param name="request">Login request</param>
        /// <returns>jwt token</returns>
        [HttpPost]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public ActionResult<string> Login([FromBody] LoginRequest request)
        {   
            if (ModelState.IsValid == false)
                return BadRequest();
            
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
