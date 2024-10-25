using PMS.Application.UseCases.Invitations.Models;
using SharedKernel.Model;

namespace PMS.Application.UseCases.Invitations
{
    public interface IInvitationService
    {
        Task<PaginatedResult<InvitationDto>> GetAllInvitationsAsync(InvitationFilterDto paginationParams,
            string tenantId);
        Task<InvitationDetailsDto> GetInvitationDetailsAsync(Guid invitationId);
        System.Threading.Tasks.Task SendInvitationAsync(InvitationSendDto invitationSendDto, string tenantId,Guid userId);
        System.Threading.Tasks.Task AcceptInvitationAsync(Guid invitationId);
        System.Threading.Tasks.Task RejectInvitationAsync(Guid invitationId);
        System.Threading.Tasks.Task CancelInvitationAsync(Guid invitationId, string tenantId,Guid userId);
        System.Threading.Tasks.Task ResendInvitationAsync(Guid invitationId, string tenantId);
        System.Threading.Tasks.Task UpdateInvitationAsync(Guid invitationId, InvitationUpdateDto invitationUpdateDto, string tenantId);
    }
}