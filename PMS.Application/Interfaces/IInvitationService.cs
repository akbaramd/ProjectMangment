using PMS.Application.DTOs;
using System;
using System.Threading.Tasks;
using SharedKernel.Model;

namespace PMS.Application.Interfaces
{
    public interface IInvitationService
    {
        Task<PaginatedResult<InvitationDto>> GetAllInvitationsAsync(InvitationFilterDto paginationParams,
            string tenantId);
        Task<InvitationDetailsDto> GetInvitationDetailsAsync(Guid invitationId);
        System.Threading.Tasks.Task SendInvitationAsync(SendInvitationDto sendInvitationDto, string tenantId,Guid userId);
        System.Threading.Tasks.Task AcceptInvitationAsync(Guid invitationId);
        System.Threading.Tasks.Task RejectInvitationAsync(Guid invitationId);
        System.Threading.Tasks.Task CancelInvitationAsync(Guid invitationId, string tenantId,Guid userId);
        System.Threading.Tasks.Task ResendInvitationAsync(Guid invitationId, string tenantId);
        System.Threading.Tasks.Task UpdateInvitationAsync(Guid invitationId, UpdateInvitationDto updateInvitationDto, string tenantId);
    }
}