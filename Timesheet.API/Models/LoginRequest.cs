using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;

namespace Timesheet.API.Models
{
    /// <summary>
    /// LoginRequest
    /// </summary>
    public class LoginRequest
    {
        /// <summary>
        /// User s Last Name
        /// </summary>
        [Required]
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
