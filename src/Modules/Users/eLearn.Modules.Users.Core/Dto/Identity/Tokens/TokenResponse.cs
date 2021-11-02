using System;

namespace eLearn.Modules.Users.Core.Dto.Identity.Tokens
{
    internal record TokenResponse(string Token, string RefreshToken, DateTime RefreshTokenExpiryTime);
}