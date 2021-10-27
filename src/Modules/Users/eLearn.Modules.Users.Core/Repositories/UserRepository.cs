using System;
using System.Threading.Tasks;
using eLearn.Modules.Users.Core.Entities;
using eLearn.Modules.Users.Core.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace eLearn.Modules.Users.Core.Repositories
{
    internal class UserRepository : IUserRepository
    {
        private readonly UserManager<ELearnUser> _userManager;

        public UserRepository(UserManager<ELearnUser> userManager)
        {
            _userManager = userManager;
        }
        
        public async Task<string> GetUserNameAsync(string userId)
        {
            var user = await _userManager.Users.FirstAsync(u => u.Id == userId);
            return user.UserName;
        }

        public async Task<string> GetUserEmailAsync(string userId)
        {
            var user = await _userManager.Users.FirstAsync(u => u.Id == userId);
            return user.Email;
        }
    }
}