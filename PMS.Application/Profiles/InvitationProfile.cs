using AutoMapper;
using PMS.Application.DTOs;
using PMS.Domain.Entities;

namespace PMS.Application.Profiles;

public class InvitationProfile : Profile
{
    public InvitationProfile()
    {
        // Map between domain entity and DTO
        CreateMap<Invitation, InvitationDto>()
            .ForMember(dest => dest.Tenant, opt => opt.MapFrom(src => src.Tenant))
            .ForMember(dest => dest.TenantId, opt => opt.MapFrom(src => src.TenantId.ToString()));
        
        // Map between DTO and domain entity
        CreateMap<InvitationDto, Invitation>()
            .ForMember(dest => dest.TenantId, opt => opt.Ignore()); // Assuming that TenantId is set elsewhere
        
        // Map Tenant to TenantDto
        CreateMap<Tenant, TenantDto>()
            .ForMember(dest => dest.Members, opt => opt.Ignore()) // We'll manually handle members
            .ForMember(dest => dest.CurrentUserRole, opt => opt.Ignore()); // Manually handle

        // Map TenantMember to TenantMemberDto and include UserDto
        CreateMap<TenantMember, TenantMemberDto>()
            .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User)); // Include User

        // Map User to UserDto
        CreateMap<ApplicationUser, UserProfileDto>();
    }
}