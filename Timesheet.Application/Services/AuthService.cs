using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Timesheet.Domain.Interfaces.IRepository;
using Timesheet.Domain.Interfaces.IService;
using Timesheet.Domain.Models;

namespace Timesheet.Application.Services
{
    public class AuthService : IAuthService
    {
        IEmployeeRepository _employeeRepository;

        public AuthService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public string Login(string lastName)
        {
            var employee = _employeeRepository.GetEmployee(lastName);
            var secret = "secret secret secret secret secret";
            var token = GenerateJwtToken(secret, employee);

            return token;
        }

        public string GenerateJwtToken(string secret, Employee employee)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.UTF8.GetBytes(secret);

            var descriptor = new SecurityTokenDescriptor()
            {
                Audience = employee.Position,
                Claims = new Dictionary<string, object> 
                { 
                    { "LastName", employee.LastName } 
                },
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateToken(descriptor);

            return tokenHandler.WriteToken(token);
        }
    }

    

    public static class UserSession
    {
        public static HashSet<string> Sessions { get; set; } = new HashSet<string>(); 
    }
}
