using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PMS.Application.Interfaces;
using PMS.Application.UseCases.Auth.Exceptions;
using PMS.Application.UseCases.Invitations.Models;
using PMS.Application.UseCases.Invitations.Specs;
using PMS.Application.UseCases.Tenant.Exceptions;
using PMS.Application.UseCases.User.Exceptions;
using PMS.Domain.BoundedContexts.TenantManagment;
using PMS.Domain.BoundedContexts.TenantManagment.Repositories;
using PMS.Domain.BoundedContexts.UserManagment.Repositories;
using SharedKernel.Model;
using UnauthorizedAccessException = PMS.Application.Exceptions.UnauthorizedAccessException;

namespace PMS.Application.UseCases.Invitations
{
    public class InvitationService : IInvitationService
    {
        private readonly IInvitationRepository _invitationRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITenantRepository _tenantRepository;
        private readonly ISmsService _smsService;
        private readonly ITenantMemberRepository _tenantMemberRepository;
        private readonly IMapper _mapper;

        public InvitationService(
            IInvitationRepository invitationRepository,
            IUserRepository userRepository,
            ITenantRepository tenantRepository,
            ISmsService smsService,
            ITenantMemberRepository tenantMemberRepository,
            IMapper mapper)
        {
            _invitationRepository = invitationRepository;
            _userRepository = userRepository;
            _tenantRepository = tenantRepository;
            _smsService = smsService;
            _tenantMemberRepository = tenantMemberRepository;
            _mapper = mapper;
        }

        // Get all invitations with pagination and search
        public async Task<PaginatedResult<InvitationDto>> GetAllInvitationsAsync(InvitationFilterDto paginationParams, string tenantId)
        {
            var tenant = await _tenantRepository.GetTenantBySubdomainAsync(tenantId);
            if (tenant == null)
            {
                throw new TenantNotFoundException();
            }

            var dto = await _invitationRepository.PaginatedAsync(new InvitationByTenantSpec(tenant.Id, paginationParams));
               

           

            var invitationDtos = _mapper.Map<PaginatedResult<InvitationDto>>(dto);
            return invitationDtos;
        }

        // Get invitation details
        public async Task<InvitationDetailsDto> GetInvitationDetailsAsync(Guid invitationId)
        {
            var invitation = await _invitationRepository.Query()
                .Include(i => i.Tenant)
                .FirstOrDefaultAsync(i => i.Id == invitationId);

            if (invitation == null || invitation.IsExpired() || invitation.Status == InvitationStatus.Canceled)
            {
                throw new InvalidInvitationTokenException("Invitation is expired or canceled.");
            }

            var userExists = await _userRepository.GetUserByPhoneNumberAsync(invitation.PhoneNumber) != null;
            var invitationDto = _mapper.Map<InvitationDto>(invitation);

            return new InvitationDetailsDto
            {
                Invitation = invitationDto,
                UserExists = userExists
            };
        }

        // Send invitation (with permission check)
        public async Task SendInvitationAsync(InvitationSendDto invitationSendDto, string tenantId, Guid userId)
        {
            var tenant = await _tenantRepository.GetTenantBySubdomainAsync(tenantId);
            if (tenant == null)
            {
                throw new TenantNotFoundException();
            }

            // Retrieve the current user as a tenantEntity member and check their permission
            var tenantMember = await _tenantMemberRepository.GetUserTenantByUserIdAndTenantIdAsync(userId, tenant.Id);
            if (tenantMember == null || !tenantMember.HasPermission("invitation:send"))
            {
                throw new UnauthorizedAccessException("You do not have permission to create invitations.");
            }

            // Check if the phone number is already a member
            var existingMember = await _tenantMemberRepository.GetTenantMemberByPhoneNumberAsync(invitationSendDto.PhoneNumber, tenant.Id);
            if (existingMember != null)
            {
                throw new InvalidOperationException("This phone number is already a member of the tenantEntity.");
            }

            // Check for existing pending invitations
            var existingInvitation = await _invitationRepository.GetInvitationByPhoneNumberAndTenantAsync(invitationSendDto.PhoneNumber, tenant.Id);
            if (existingInvitation !=null && (existingInvitation.Status == InvitationStatus.Pending || existingInvitation.Status == InvitationStatus.Accepted) && !existingInvitation.IsExpired())
            {
                throw new InvalidOperationException("There is already a pending invitation for this phone number.");
            }

            var expirationDuration = invitationSendDto.ExpirationDuration == TimeSpan.Zero
                ? TimeSpan.FromDays(7) // Default to 7 days
                : invitationSendDto.ExpirationDuration;

            if (existingInvitation != null && existingInvitation.IsExpired())
            {
                // If an expired invitation exists, renew it
                existingInvitation.Renew(expirationDuration);
                await _invitationRepository.UpdateAsync(existingInvitation);
            }
            else
            {
                // Create a new invitation
                existingInvitation = new ProjectInvitationEntity(invitationSendDto.PhoneNumber, tenant, expirationDuration);
                await _invitationRepository.AddAsync(existingInvitation);
            }

            var invitationLink = $"http://{tenant.Subdomain}.yourdomain.com/invitation/{existingInvitation.Id}";
            var message = $"You have been invited to join {tenant.Name}. Click here: {invitationLink}";
            await _smsService.SendSmsAsync(invitationSendDto.PhoneNumber, message);
        }

        // Cancel invitation (with permission check)
        public async Task CancelInvitationAsync(Guid invitationId, string tenantId, Guid userId)
        {
            var tenant = await _tenantRepository.GetTenantBySubdomainAsync(tenantId);
            if (tenant == null)
            {
                throw new TenantNotFoundException();
            }

            // Retrieve the current user as a tenantEntity member and check their permission
            var tenantMember = await _tenantMemberRepository.GetUserTenantByUserIdAndTenantIdAsync(userId, tenant.Id);
            if (tenantMember == null || !tenantMember.HasPermission("invitation:cancel"))
            {
                throw new UnauthorizedAccessException("You do not have permission to cancel invitations.");
            }

            var invitation = await _invitationRepository.GetByIdAsync(invitationId);
            if (invitation == null || invitation.TenantId != tenant.Id || invitation.Status == InvitationStatus.Canceled)
            {
                throw new InvalidInvitationTokenException("Invalid or already canceled invitation.");
            }

            invitation.Cancel();
            await _invitationRepository.UpdateAsync(invitation);
        }

        // Resend invitation
        public async Task ResendInvitationAsync(Guid invitationId, string tenantId)
        {
            var tenant = await _tenantRepository.GetTenantBySubdomainAsync(tenantId);
            if (tenant == null)
            {
                throw new TenantNotFoundException();
            }

            var invitation = await _invitationRepository.GetByIdAsync(invitationId);
            if (invitation == null || invitation.IsExpired() || invitation.Status == InvitationStatus.Canceled)
            {
                throw new InvalidInvitationTokenException("Cannot resend an expired or canceled invitation.");
            }

            var invitationLink = $"http://{tenant.Subdomain}.yourdomain.com/invitation/{invitation.Id}";
            var message = $"Reminder: You have been invited to join {tenant.Name}. Click here: {invitationLink}";
            await _smsService.SendSmsAsync(invitation.PhoneNumber, message);
        }

        // Accept invitation
        public async Task AcceptInvitationAsync(Guid invitationId)
        {
            var invitation = await _invitationRepository.GetByIdAsync(invitationId);
            if (invitation == null || invitation.IsExpired() || invitation.Status == InvitationStatus.Canceled)
            {
                throw new InvalidInvitationTokenException("Invitation is expired or canceled.");
            }

            var user = await _userRepository.GetUserByPhoneNumberAsync(invitation.PhoneNumber);
            if (user == null)
            {
                throw new UserNotFoundException("User not found.");
            }

            var tenant = await _tenantRepository.GetByIdAsync(invitation.TenantId);
            if (tenant == null)
            {
                throw new TenantNotFoundException("Tenant not found.");
            }

            // Add the user to the tenantEntity
            await _tenantMemberRepository.AddAsync(new TenantMemberEntity(user, tenant));

            invitation.Accept();
            await _invitationRepository.UpdateAsync(invitation);
        }

        // Reject invitation
        public async Task RejectInvitationAsync(Guid invitationId)
        {
            var invitation = await _invitationRepository.GetByIdAsync(invitationId);
            if (invitation == null || invitation.IsExpired() || invitation.Status == InvitationStatus.Canceled)
            {
                throw new InvalidInvitationTokenException("Invitation is expired or canceled.");
            }

            invitation.Reject();
            await _invitationRepository.UpdateAsync(invitation);
        }

        // Update invitation
        public async Task UpdateInvitationAsync(Guid invitationId, InvitationUpdateDto invitationUpdateDto, string tenantId)
        {
            var tenant = await _tenantRepository.GetTenantBySubdomainAsync(tenantId);
            if (tenant == null)
            {
                throw new TenantNotFoundException();
            }

            var invitation = await _invitationRepository.GetByIdAsync(invitationId);
            if (invitation == null || invitation.TenantId != tenant.Id || invitation.Status == InvitationStatus.Accepted || invitation.Status == InvitationStatus.Canceled || invitation.IsExpired())
            {
                throw new InvalidInvitationTokenException("Cannot update an accepted, canceled, or expired invitation.");
            }

            if (invitationUpdateDto.NewExpirationDuration.HasValue)
            {
                invitation.ExpirationDate = DateTime.UtcNow.Add(invitationUpdateDto.NewExpirationDuration.Value);
            }

            await _invitationRepository.UpdateAsync(invitation);
        }
    }
}