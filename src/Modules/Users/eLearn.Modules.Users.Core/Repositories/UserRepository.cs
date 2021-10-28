using System.Threading.Tasks;
using eLearn.Modules.Users.Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace eLearn.Modules.Users.Core.Repositories
{
    internal class UserRepository : IUserRepository
    {
        private readonly UserManager<User> _userManager;

        public UserRepository(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<User> GetAsync(string email)
            => await _userManager.FindByEmailAsync(email);

        public async Task AddAsync(User user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                //GenerateEmailConfirmationTokenAsync
                //await _userManager.AddToRoleAsync(user, "customer");
                //await _signInManager.SignInAsync(user, isPersistent: false);
                //_logger.LogInformation(3, "User created a new account with password.");
            }
            else
            {
                // _logger.LogInformation(3, @"{result.Errors}");
            }
        }
    }
}