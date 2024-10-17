using Microsoft.EntityFrameworkCore;
using PMS.Application.DTOs;
using PMS.Application.Exceptions;
using PMS.Application.Interfaces;
using PMS.Domain.Entities;
using PMS.Domain.Repositories;

namespace PMS.Application.Services;

public class InvitationService : IInvitationService
{
    private readonly IInvitationRepository invitationRepository;
    private readonly IUserRepository userRepository;
    private readonly ITenantRepository tenantRepository;
    private readonly ISmsService smsService;
    private readonly IUserTenantRepository userTenantRepository;

    public InvitationService(
        IInvitationRepository invitationRepository,
        IUserRepository userRepository,
        ITenantRepository tenantRepository,
        ISmsService smsService,
        IUserTenantRepository userTenantRepository)
    {
        this.invitationRepository = invitationRepository;
        this.userRepository = userRepository;
        this.tenantRepository = tenantRepository;
        this.smsService = smsService;
        this.userTenantRepository = userTenantRepository;
    }

    // Get all invitations with pagination and search
    public async Task<PaginatedList<InvitationDto>> GetAllInvitationsAsync(PaginationParams paginationParams, string tenantId)
    {
        var tenant = await tenantRepository.GetTenantBySubdomainAsync(tenantId);
        if (tenant == null)
        {
            throw new TenantNotFoundException();
        }

        var query = invitationRepository.Query()
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
            .Select(i => new InvitationDto
            {
                Id = i.Id,
                PhoneNumber = i.PhoneNumber,
                TenantId = i.TenantId.ToString(),
                CreatedAt = i.CreatedAt,
                IsAccepted = i.IsAccepted,
                AcceptedAt = i.AcceptedAt,
                ExpirationDate = i.ExpirationDate,
                IsCanceled = i.IsCanceled,
                CanceledAt = i.CanceledAt
            })
            .ToListAsync();

        return new PaginatedList<InvitationDto>(invitations, totalCount, paginationParams.PageNumber, paginationParams.PageSize);
    }

    // Get pending invitations
    public async Task<PaginatedList<InvitationDto>> GetPendingInvitationsAsync(PaginationParams paginationParams, string tenantId)
    {
        var tenant = await tenantRepository.GetTenantBySubdomainAsync(tenantId);
        if (tenant == null)
        {
            throw new TenantNotFoundException();
        }

        var query = invitationRepository.Query()
            .Where(i => i.TenantId == tenant.Id && !i.IsAccepted && !i.IsCanceled && !i.IsExpired());

        if (!string.IsNullOrEmpty(paginationParams.Search))
        {
            query = query.Where(i => i.PhoneNumber.Contains(paginationParams.Search));
        }

        var totalCount = await query.CountAsync();

        var invitations = await query
            .OrderByDescending(i => i.CreatedAt)
            .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
            .Take(paginationParams.PageSize)
            .Select(i => new InvitationDto
            {
                Id = i.Id,
                PhoneNumber = i.PhoneNumber,
                TenantId = i.TenantId.ToString(),
                CreatedAt = i.CreatedAt,
                IsAccepted = i.IsAccepted,
                ExpirationDate = i.ExpirationDate,
                IsCanceled = i.IsCanceled
            })
            .ToListAsync();

        return new PaginatedList<InvitationDto>(invitations, totalCount, paginationParams.PageNumber, paginationParams.PageSize);
    }

    // Get invitation detail
    public async Task<InvitationDetailsDto> GetInvitationDetailsAsync(Guid invitationId)
    {
        var invitation = await invitationRepository.GetByIdAsync(invitationId);
        if (invitation == null)
        {
            throw new InvalidInvitationTokenException("Invitation not found.");
        }

        if (invitation.IsExpired() || invitation.IsCanceled)
        {
            throw new InvalidInvitationTokenException("Invitation is expired or canceled.");
        }

        var userExists = await userRepository.GetUserByPhoneNumberAsync(invitation.PhoneNumber) != null;

        return new InvitationDetailsDto
        {
            PhoneNumber = invitation.PhoneNumber,
            TenantId = invitation.TenantId.ToString(),
            UserExists = userExists
        };
    }

    // Send invitation
    public async Task SendInvitationAsync(SendInvitationDto sendInvitationDto, string tenantId)
    {
        var tenant = await tenantRepository.GetTenantBySubdomainAsync(tenantId);
        if (tenant == null)
        {
            throw new TenantNotFoundException();
        }

        var expirationDuration = sendInvitationDto.ExpirationDuration;
        if (expirationDuration == TimeSpan.Zero)
        {
            expirationDuration = TimeSpan.FromDays(7); // Default to 7 days
        }

        var existingInvitation = await invitationRepository.GetInvitationByPhoneNumberAndTenantAsync(sendInvitationDto.PhoneNumber, tenant.Id);

        if (existingInvitation != null)
        {
            existingInvitation.Renew(expirationDuration);
            await invitationRepository.UpdateAsync(existingInvitation);
        }
        else
        {
            existingInvitation = new Invitation(sendInvitationDto.PhoneNumber, tenant, expirationDuration);
            await invitationRepository.AddAsync(existingInvitation);
        }

        var invitationLink = $"http://{tenant.Subdomain}.yourdomain.com/invitation/{existingInvitation.Id}";

        var message = $"You have been invited to join {tenant.Name}. Click here: {invitationLink}";
        await smsService.SendSmsAsync(sendInvitationDto.PhoneNumber, message);
    }

    // Accept invitation
    public async Task AcceptInvitationAsync(Guid invitationId)
    {
        var invitation = await invitationRepository.GetByIdAsync(invitationId);
        if (invitation == null)
        {
            throw new InvalidInvitationTokenException("Invitation not found.");
        }

        if (invitation.IsExpired() || invitation.IsCanceled)
        {
            throw new InvalidInvitationTokenException("Invitation is expired or canceled.");
        }

        var user = await userRepository.GetUserByPhoneNumberAsync(invitation.PhoneNumber);
        if (user == null)
        {
            throw new UserNotFoundException("User not found.");
        }

        var tenant = await tenantRepository.GetByIdAsync(invitation.TenantId);
        if (tenant == null)
        {
            throw new TenantNotFoundException("Tenant not found.");
        }

        // Add user to tenant
        await userTenantRepository.AddAsync(new UserTenant(user, tenant, UserTenantRole.Employee));

        invitation.Accept();
        await invitationRepository.UpdateAsync(invitation);
    }

    // Reject invitation
    public async Task RejectInvitationAsync(Guid invitationId)
    {
        var invitation = await invitationRepository.GetByIdAsync(invitationId);
        if (invitation == null)
        {
            throw new InvalidInvitationTokenException("Invitation not found.");
        }

        if (invitation.IsExpired() || invitation.IsCanceled)
        {
            throw new InvalidInvitationTokenException("Invitation is expired or canceled.");
        }

        var user = await userRepository.GetUserByPhoneNumberAsync(invitation.PhoneNumber);
        if (user == null)
        {
            throw new UserNotFoundException("User not found.");
        }

        invitation.Reject();
        await invitationRepository.UpdateAsync(invitation);
    }

    // Cancel invitation
    public async Task CancelInvitationAsync(Guid invitationId, string tenantId)
    {
        var tenant = await tenantRepository.GetTenantBySubdomainAsync(tenantId);
        if (tenant == null)
        {
            throw new TenantNotFoundException("Tenant not found.");
        }

        var invitation = await invitationRepository.GetByIdAsync(invitationId);
        if (invitation == null || invitation.TenantId != tenant.Id)
        {
            throw new InvalidInvitationTokenException("Invitation not found.");
        }

        if (invitation.IsCanceled)
        {
            throw new InvalidOperationException("Invitation is already canceled.");
        }

        invitation.IsCanceled = true;
        invitation.CanceledAt = DateTime.UtcNow;

        await invitationRepository.UpdateAsync(invitation);
    }

    // Resend invitation
    public async Task ResendInvitationAsync(Guid invitationId, string tenantId)
    {
        var tenant = await tenantRepository.GetTenantBySubdomainAsync(tenantId);
        if (tenant == null)
        {
            throw new TenantNotFoundException("Tenant not found.");
        }

        var invitation = await invitationRepository.GetByIdAsync(invitationId);
        if (invitation == null || invitation.TenantId != tenant.Id)
        {
            throw new InvalidInvitationTokenException("Invitation not found.");
        }

        if (invitation.IsExpired() || invitation.IsCanceled)
        {
            throw new InvalidOperationException("Cannot resend an expired or canceled invitation.");
        }

        var invitationLink = $"http://{tenant.Subdomain}.yourdomain.com/invitation/{invitation.Id}";

        var message = $"Reminder: You have been invited to join {tenant.Name}. Click here: {invitationLink}";
        await smsService.SendSmsAsync(invitation.PhoneNumber, message);
    }

    // Update invitation
    public async Task UpdateInvitationAsync(Guid invitationId, UpdateInvitationDto updateInvitationDto, string tenantId)
    {
        var tenant = await tenantRepository.GetTenantBySubdomainAsync(tenantId);
        if (tenant == null)
        {
            throw new TenantNotFoundException("Tenant not found.");
        }

        var invitation = await invitationRepository.GetByIdAsync(invitationId);
        if (invitation == null || invitation.TenantId != tenant.Id)
        {
            throw new InvalidInvitationTokenException("Invitation not found.");
        }

        if (invitation.IsAccepted || invitation.IsCanceled || invitation.IsExpired())
        {
            throw new InvalidOperationException("Cannot update an accepted, canceled, or expired invitation.");
        }

        if (updateInvitationDto.NewExpirationDuration.HasValue)
        {
            invitation.ExpirationDate = DateTime.UtcNow.Add(updateInvitationDto.NewExpirationDuration.Value);
        }

        // Update other fields as necessary

        await invitationRepository.UpdateAsync(invitation);
    }
}
