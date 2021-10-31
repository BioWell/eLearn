using System.Threading.Tasks;
using eLearn.Modules.Users.Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace eLearn.Modules.Users.Core.Repositories
{
    internal class UserRepository : IUserRepository
    {
        private readonly UserManager<AppUser> _userManager;

        public UserRepository(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<AppUser> GetAsync(string email)
            => await _userManager.FindByEmailAsync(email);

        public async Task AddAsync(AppUser appUser, string password)
        {
            var result = await _userManager.CreateAsync(appUser, password);

            if (result.Succeeded)
            {
                //GenerateEmailConfirmationTokenAsync
                //await _userManager.AddToRoleAsync(appUser, "customer");
                //await _signInManager.SignInAsync(appUser, isPersistent: false);
                //_logger.LogInformation(3, "AppUser created a new account with password.");
            }
            else
            {
                // _logger.LogInformation(3, @"{result.Errors}");
            }
        }
    }
}