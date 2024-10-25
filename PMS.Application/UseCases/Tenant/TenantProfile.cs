using AutoMapper;
using PMS.Application.UseCases.Tenant.Models;
using PMS.Domain.BoundedContexts.TenantManagment;
using SharedKernel.Model;

namespace PMS.Application.UseCases.Tenant
{
    public class TenantProfile : Profile
    {
        public TenantProfile()
        {
            // Map Tenant to TenantDto
            CreateMap<TenantEntity, TenantDto>()
                .ForMember(dest => dest.Members, opt => opt.Ignore()); // We'll manually handle members

            // Map TenantMember to TenantMemberDto and include UserDto
            CreateMap<TenantMemberEntity, TenantMemberDto>()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User)); // Include User


            CreateMap<PaginatedResult<TenantMemberEntity>, PaginatedResult<TenantMemberDto>>();

        }
    }
}