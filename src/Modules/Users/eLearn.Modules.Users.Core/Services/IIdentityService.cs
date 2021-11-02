﻿using System.Threading.Tasks;
using eLearn.Modules.Users.Core.Commands;
using Shared.Infrastructure.Wrapper;

namespace eLearn.Modules.Users.Core.Services
{
    internal interface IIdentityService
    {
        Task<IResult> RegisterAsync(RegisterRequest request, string origin);
        
        Task<IResult> LoginAsync(RegisterRequest request);

        Task<IResult<string>> ConfirmEmailAsync(string userId, string code);

        Task<IResult<string>> ConfirmPhoneNumberAsync(string userId, string code);

        Task<IResult> ForgotPasswordAsync(ForgotPasswordRequest request, string origin);

        Task<IResult> ResetPasswordAsync(ResetPasswordRequest request);
    }
}