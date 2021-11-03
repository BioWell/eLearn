using System;
using MediatR;

namespace eLearn.Modules.Users.Core.Commands
{
    internal class RoleRequest : IRequest
    {
        public string Id { get; set; } = String.Empty;

        public string Name { get; set; } = String.Empty;

        public string Description { get; set; } = String.Empty;
    }
}