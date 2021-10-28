using System.Threading.Tasks;
using eLearn.Modules.Users.Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace eLearn.Modules.Users.Core.Repositories
{
    internal class UserRepository : IUserRepository
    {
        private readonly UserManager<ELearnUser> _userManager;

        public UserRepository(UserManager<ELearnUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ELearnUser> GetAsync(string email) 
            => await _userManager.FindByEmailAsync(email);

        public async Task AddAsync(ELearnUser user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            
            if (result.Succeeded)
            {
                //GenerateEmailConfirmationTokenAsync
            }
            else
            {
                // result.Errors
            }
        }
    }
}