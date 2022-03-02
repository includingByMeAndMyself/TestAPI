using FluentValidation;
using System;

namespace Timesheet.API.Models
{
    public class LoginRequest
    {
        public string LastName { get; set; }
    }

    public class LoginRequestFluentValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestFluentValidator()
        {
            RuleFor(x => x.LastName)
                .NotEmpty();
        }
    }
}
