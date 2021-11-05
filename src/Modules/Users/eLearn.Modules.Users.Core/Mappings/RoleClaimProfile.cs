using AutoMapper;
using eLearn.Modules.Users.Core.Commands;
using eLearn.Modules.Users.Core.Dto.Identity.Roles;
using eLearn.Modules.Users.Core.Entities;

namespace eLearn.Modules.Users.Core.Mappings
{
    internal class RoleClaimProfile : Profile
    {
        public RoleClaimProfile()
        {
            CreateMap<RoleClaimResponse, AppRoleClaim>()
                .ForMember(nameof(AppRoleClaim.ClaimType), opt => opt.MapFrom(c => c.Type))
                .ForMember(nameof(AppRoleClaim.ClaimValue), opt => opt.MapFrom(c => c.Value))
                .ReverseMap();

            CreateMap<RoleClaimRequest, AppRoleClaim>()
                .ForMember(nameof(AppRoleClaim.ClaimType), opt => opt.MapFrom(c => c.Type))
                .ForMember(nameof(AppRoleClaim.ClaimValue), opt => opt.MapFrom(c => c.Value))
                .ReverseMap();

            CreateMap<RoleClaimModel, RoleClaimRequest>();
            CreateMap<RoleClaimModel, AppRoleClaim>().ReverseMap();
            CreateMap<RoleClaimModel, RoleClaimResponse>().ReverseMap();
        }
    }
}