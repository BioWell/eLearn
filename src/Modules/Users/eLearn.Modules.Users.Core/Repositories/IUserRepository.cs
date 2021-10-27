using System;
using System.Threading.Tasks;
using eLearn.Modules.Users.Core.Entities;

namespace eLearn.Modules.Users.Core.Repositories
{
    internal interface IUserRepository
    {
        Task<string> GetUserNameAsync(string userId);
        Task<string> GetUserEmailAsync(string email);
    }
}