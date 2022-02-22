﻿using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timesheet.API
{
    public class JwtAuthMiddleware
    {
        private readonly RequestDelegate _next;
        public JwtAuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            var authHeader = context.Request.Headers["Authorization"]
                .FirstOrDefault();

            if(authHeader != null)
            {
                var secret = "as dsd aas asas assd sdsd";
                var key = Encoding.UTF8.GetBytes(secret);

                var token = authHeader.Split(" ").Last();
                var tokenHandler = new JwtSecurityTokenHandler();

                var validationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateAudience = false,
                    ValidateIssuer = false
                };

                tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;

                context.Items["LastName"] = jwtToken.Claims
                    .First(x => x.Type == "LastName")
                    .Value;
            }

            await _next(context);
        }
    }
}
