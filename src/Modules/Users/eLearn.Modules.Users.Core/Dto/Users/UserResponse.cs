using System;

namespace eLearn.Modules.Users.Core.Dto.Users
{
    internal class UserResponse
    {
        public string Id { get; set; } = String.Empty;

        public string UserName { get; set; } = String.Empty;

        public string FirstName { get; set; } = String.Empty;

        public string LastName { get; set; } = String.Empty;

        public string Email { get; set; } = String.Empty;

        public bool IsActive { get; set; } = true;

        public bool? EmailConfirmed { get; set; }

        public string? PhoneNumber { get; set; }

        public bool? PhoneNumberConfirmed { get; set; }

        public string? ProfilePictureUrl { get; set; }
    }
}