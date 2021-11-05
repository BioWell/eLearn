using System;
using System.ComponentModel.DataAnnotations;
using MediatR;

namespace eLearn.Modules.Users.Core.Commands
{
    internal class RoleRequest : IRequest
    {
        public string? Id { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        public string Name { get; set; } = String.Empty;

        public string? Description { get; set; }
    }
}