using AutoMapper;
using Bonyan.Layer.Domain.Model;
using PMS.Application.UseCases.Invitations.Models;
using PMS.Domain.BoundedContexts.TenantManagement;

namespace PMS.Application.UseCases.Invitations
{
    public class InvitationProfile : Profile
    {
        public InvitationProfile()
        {
            // Map between domain entity and DTO
            CreateMap<ProjectInvitationEntity, InvitationDto>()
                .ForMember(dest => dest.TenantId, opt => opt.MapFrom(src => src.TenantId.ToString()));

            // Map between DTO and domain entity
            CreateMap<InvitationDto, ProjectInvitationEntity>()
                .ForMember(dest => dest.TenantId, opt => opt.Ignore()); // Assuming that TenantId is set elsewhere
            
            CreateMap<PaginatedResult<ProjectInvitationEntity>, PaginatedResult<InvitationDto>>().ReverseMap();
        }
    }
}