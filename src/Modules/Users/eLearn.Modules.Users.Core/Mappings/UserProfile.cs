using AutoMapper;
using eLearn.Modules.Users.Core.Commands;
using eLearn.Modules.Users.Core.Dto.Users;
using eLearn.Modules.Users.Core.Entities;

namespace eLearn.Modules.Users.Core.Mappings
{
    internal class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserResponse, AppUser>().ReverseMap();
            CreateMap<UpdateUserRequest, AppUser>().ReverseMap();
        }
    }
}