using System;

namespace eLearn.Modules.Users.Core.Dto.Identity.Roles
{
    internal class RoleClaimResponse
    {
        public int Id { get; set; }

        public string RoleId { get; set; } = String.Empty;

        public string Type { get; set; } = String.Empty;

        public string Value { get; set; } = String.Empty;

        public string? Description { get; set; }

        public string? Group { get; set; }
    }
}