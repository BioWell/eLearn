using System;
using System.ComponentModel.DataAnnotations;
using MediatR;

namespace eLearn.Modules.Users.Core.Commands
{
    internal class SignIn : IRequest
    {
        [Required] [EmailAddress] public string Email { get; set; } = string.Empty;
        [Required] public string Password { get; set; } = string.Empty;
        public Guid Id { get; init; } = Guid.NewGuid();
    }
}