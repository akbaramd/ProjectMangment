using AutoMapper;
using Bonyan.Layer.Domain.Model;
using Bonyan.TenantManagement.Domain;
using PMS.Application.UseCases.Tenants.Models;
using PMS.Domain.BoundedContexts.TenantManagement;

namespace PMS.Application.UseCases.Tenants
{
    public class TenantProfile : Profile
    {
        public TenantProfile()
        {
            // Map Tenants to TenantDto
            CreateMap<Tenant, TenantDto>()
                .ForMember(dest => dest.Members, opt => opt.Ignore()); // We'll manually handle members

            // Map TenantMember to TenantMemberDto and include UserDto
            CreateMap<TenantMemberEntity, TenantMemberDto>()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.UserEntity)); // Include UserEntity


            CreateMap<PaginatedResult<TenantMemberEntity>, PaginatedResult<TenantMemberDto>>();

        }
    }
}