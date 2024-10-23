using AutoMapper;
using PMS.Application.DTOs;
using PMS.Domain.Entities;
using SharedKernel.Model;

namespace PMS.Application.Profiles
{
    public class TenantProfile : Profile
    {
        public TenantProfile()
        {
            // Map Tenant to TenantDto
            CreateMap<Tenant, TenantDto>()
                .ForMember(dest => dest.Members, opt => opt.Ignore()); // We'll manually handle members

            // Map TenantMember to TenantMemberDto and include UserDto
            CreateMap<TenantMember, TenantMemberDto>()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User)); // Include User


            CreateMap<PaginatedResult<TenantMember>, PaginatedResult<TenantMemberDto>>();

        }
    }
}