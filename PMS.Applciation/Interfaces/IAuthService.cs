using System.Threading.Tasks;
using PMS.Application.DTOs;

namespace PMS.Application.Interfaces
{
    public interface IAuthService
    {
        Task RegisterAsync(RegisterDto registerDto);
        Task SendInvitationAsync(SendInvitationDto sendInvitationDto, string tenantId);
        Task<InvitationDetailsDto> GetInvitationDetailsAsync(Guid invitationToken);
        Task AcceptInvitationAsync(AcceptInvitationDto acceptInvitationDto);
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto, string tenantId);
    }
}