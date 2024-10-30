using Bonyan.Layer.Domain.Model;
using PMS.Application.UseCases.Invitations.Models;

namespace PMS.Application.UseCases.Invitations
{
    public interface IInvitationService
    {
        Task<PaginatedResult<InvitationDto>> GetAllInvitationsAsync(InvitationFilterDto paginationParams);
        Task<InvitationDetailsDto> GetInvitationDetailsAsync(Guid invitationId);
        System.Threading.Tasks.Task SendInvitationAsync(InvitationSendDto invitationSendDto,Guid userId);
        System.Threading.Tasks.Task AcceptInvitationAsync(Guid invitationId);
        System.Threading.Tasks.Task RejectInvitationAsync(Guid invitationId);
        System.Threading.Tasks.Task CancelInvitationAsync(Guid invitationId,Guid userId);
        System.Threading.Tasks.Task ResendInvitationAsync(Guid invitationId);
        System.Threading.Tasks.Task UpdateInvitationAsync(Guid invitationId, InvitationUpdateDto invitationUpdateDto);
    }
}