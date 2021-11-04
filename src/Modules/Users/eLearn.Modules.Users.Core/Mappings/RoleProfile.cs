using AutoMapper;
using eLearn.Modules.Users.Core.Dto.Identity.Roles;
using eLearn.Modules.Users.Core.Entities;

namespace eLearn.Modules.Users.Core.Mappings
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<RoleResponse, AppRole>().ReverseMap();
        }
    }
}