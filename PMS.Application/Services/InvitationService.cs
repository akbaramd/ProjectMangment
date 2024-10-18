using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PMS.Application.DTOs;
using PMS.Application.Exceptions;
using PMS.Application.Interfaces;
using PMS.Domain.Entities;
using PMS.Domain.Repositories;

namespace PMS.Application.Services
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
        public async Task<PaginatedList<InvitationDto>> GetAllInvitationsAsync(PaginationParams paginationParams, string tenantId)
        {
            var tenant = await _tenantRepository.GetTenantBySubdomainAsync(tenantId);
            if (tenant == null)
            {
                throw new TenantNotFoundException();
            }

            var query = _invitationRepository.Query()
                .Include(i => i.Tenant)
                .Where(i => i.TenantId == tenant.Id);

            if (!string.IsNullOrEmpty(paginationParams.Search))
            {
                query = query.Where(i => i.PhoneNumber.Contains(paginationParams.Search));
            }

            var totalCount = await query.CountAsync();

            var invitations = await query
                .OrderByDescending(i => i.CreatedAt)
                .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
                .Take(paginationParams.PageSize)
                .ToListAsync();

            var invitationDtos = _mapper.Map<List<InvitationDto>>(invitations);
            return new PaginatedList<InvitationDto>(invitationDtos, totalCount, paginationParams.PageNumber, paginationParams.PageSize);
        }

        // Get invitation details
        public async Task<InvitationDetailsDto> GetInvitationDetailsAsync(Guid invitationId)
        {
            var invitation = await _invitationRepository.Query()
                .Include(i => i.Tenant)
                .FirstOrDefaultAsync(i => i.Id == invitationId);

            if (invitation == null || invitation.IsExpired() || invitation.Status == InvitationStatus.Cancel)
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

        // Send invitation
        public async Task SendInvitationAsync(SendInvitationDto sendInvitationDto, string tenantId)
        {
            var tenant = await _tenantRepository.GetTenantBySubdomainAsync(tenantId);
            if (tenant == null)
            {
                throw new TenantNotFoundException();
            }

            // Check if the phone number is already a member
            var existingMember = await _tenantMemberRepository.GetTenantMemberByPhoneNumberAsync(sendInvitationDto.PhoneNumber, tenant.Id);
            if (existingMember != null)
            {
                throw new InvalidOperationException("This phone number is already a member of the tenant.");
            }

            // Check for existing pending invitations
            var existingInvitation = await _invitationRepository.GetInvitationByPhoneNumberAndTenantAsync(sendInvitationDto.PhoneNumber, tenant.Id);
            if (existingInvitation is { Status: InvitationStatus.Pending or InvitationStatus.Accepted } && !existingInvitation.IsExpired())
            {
                throw new InvalidOperationException("There is already a pending invitation for this phone number.");
            }

            var expirationDuration = sendInvitationDto.ExpirationDuration == TimeSpan.Zero
                ? TimeSpan.FromDays(7) // Default to 7 days
                : sendInvitationDto.ExpirationDuration;

            if (existingInvitation != null && existingInvitation.IsExpired())
            {
                // If an expired invitation exists, renew it
                existingInvitation.Renew(expirationDuration);
                await _invitationRepository.UpdateAsync(existingInvitation);
            }
            else
            {
                // Create a new invitation
                var newInvitation = new Invitation(sendInvitationDto.PhoneNumber, tenant, expirationDuration);
                await _invitationRepository.AddAsync(newInvitation);
            }

            var invitationLink = $"http://{tenant.Subdomain}.yourdomain.com/invitation/{existingInvitation.Id}";
            var message = $"You have been invited to join {tenant.Name}. Click here: {invitationLink}";
            await _smsService.SendSmsAsync(sendInvitationDto.PhoneNumber, message);
        }

        // Accept invitation
        public async Task AcceptInvitationAsync(Guid invitationId)
        {
            var invitation = await _invitationRepository.GetByIdAsync(invitationId);
            if (invitation == null || invitation.IsExpired() || invitation.Status == InvitationStatus.Cancel)
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

            // Add the user to the tenant
            await _tenantMemberRepository.AddAsync(new TenantMember(user, tenant, TenantMemberRole.Employee));

            invitation.Accept();
            await _invitationRepository.UpdateAsync(invitation);
        }

        // Reject invitation
        public async Task RejectInvitationAsync(Guid invitationId)
        {
            var invitation = await _invitationRepository.GetByIdAsync(invitationId);
            if (invitation == null || invitation.IsExpired() || invitation.Status == InvitationStatus.Cancel)
            {
                throw new InvalidInvitationTokenException("Invitation is expired or canceled.");
            }

            invitation.Reject();
            await _invitationRepository.UpdateAsync(invitation);
        }

        // Cancel invitation
        public async Task CancelInvitationAsync(Guid invitationId, string tenantId)
        {
            var tenant = await _tenantRepository.GetTenantBySubdomainAsync(tenantId);
            if (tenant == null)
            {
                throw new TenantNotFoundException();
            }

            var invitation = await _invitationRepository.GetByIdAsync(invitationId);
            if (invitation == null || invitation.TenantId != tenant.Id || invitation.Status == InvitationStatus.Cancel)
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
            if (invitation == null || invitation.IsExpired() || invitation.Status == InvitationStatus.Cancel)
            {
                throw new InvalidInvitationTokenException("Cannot resend an expired or canceled invitation.");
            }

            var invitationLink = $"http://{tenant.Subdomain}.yourdomain.com/invitation/{invitation.Id}";
            var message = $"Reminder: You have been invited to join {tenant.Name}. Click here: {invitationLink}";
            await _smsService.SendSmsAsync(invitation.PhoneNumber, message);
        }

        // Update invitation
        public async Task UpdateInvitationAsync(Guid invitationId, UpdateInvitationDto updateInvitationDto, string tenantId)
        {
            var tenant = await _tenantRepository.GetTenantBySubdomainAsync(tenantId);
            if (tenant == null)
            {
                throw new TenantNotFoundException();
            }

            var invitation = await _invitationRepository.GetByIdAsync(invitationId);
            if (invitation == null || invitation.TenantId != tenant.Id || invitation.Status == InvitationStatus.Accepted || invitation.Status == InvitationStatus.Cancel || invitation.IsExpired())
            {
                throw new InvalidInvitationTokenException("Cannot update an accepted, canceled, or expired invitation.");
            }

            if (updateInvitationDto.NewExpirationDuration.HasValue)
            {
                invitation.ExpirationDate = DateTime.UtcNow.Add(updateInvitationDto.NewExpirationDuration.Value);
            }

            await _invitationRepository.UpdateAsync(invitation);
        }
    }
}