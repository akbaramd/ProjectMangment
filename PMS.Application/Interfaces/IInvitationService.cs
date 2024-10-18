using PMS.Application.DTOs;
using System;
using System.Threading.Tasks;

namespace PMS.Application.Interfaces
{
    public interface IInvitationService
    {
        Task<PaginatedList<InvitationDto>> GetAllInvitationsAsync(PaginationParams paginationParams, string tenantId);
        Task<InvitationDetailsDto> GetInvitationDetailsAsync(Guid invitationId);
        Task SendInvitationAsync(SendInvitationDto sendInvitationDto, string tenantId);
        Task AcceptInvitationAsync(Guid invitationId);
        Task RejectInvitationAsync(Guid invitationId);
        Task CancelInvitationAsync(Guid invitationId, string tenantId);
        Task ResendInvitationAsync(Guid invitationId, string tenantId);
        Task UpdateInvitationAsync(Guid invitationId, UpdateInvitationDto updateInvitationDto, string tenantId);
    }
}