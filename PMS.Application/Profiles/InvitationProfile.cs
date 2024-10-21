using AutoMapper;
using PMS.Application.DTOs;
using PMS.Domain.Entities;

namespace PMS.Application.Profiles
{
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
        }
    }
}