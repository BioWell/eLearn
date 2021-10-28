using System;
using System.Threading.Tasks;
using eLearn.Modules.Users.Core.Entities;

namespace eLearn.Modules.Users.Core.Repositories
{
    internal interface IUserRepository
    {
        Task<ELearnUser> GetAsync(string email);
        Task AddAsync(ELearnUser user, string password);
    }
}