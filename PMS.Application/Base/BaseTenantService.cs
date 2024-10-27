using System.Security.Claims;
using Bonyan.DomainDrivenDesign.Domain.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using PMS.Application.UseCases.Tenant.Exceptions;
using PMS.Domain.BoundedContexts.TenantManagement;
using PMS.Domain.BoundedContexts.TenantManagement.Repositories;
using PMS.Domain.BoundedContexts.UserManagment;
using PMS.Domain.BoundedContexts.UserManagment.Repositories;

namespace PMS.Application.Base
{
    public abstract class BaseTenantService
    {
        private readonly IServiceProvider _serviceProvider;

        protected ApplicationUser CurrentUser { get; private set; }
        protected TenantEntity CurrentTenant { get; private set; }

        // Constructor that takes IServiceProvider to resolve dependencies
        protected BaseTenantService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            InitializeUserAndTenant().Wait(); // Asynchronous initialization.
        }

        // Method to initialize the CurrentUser and Tenant, if possible
        private async Task InitializeUserAndTenant()
        {
            // Resolve ClaimsPrincipal to get the current user ID from claims
            var user = _serviceProvider.GetService<IHttpContextAccessor>();
            var userIdClaim = user?.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!string.IsNullOrEmpty(userIdClaim))
            {
               var  currentUserId = Guid.Parse(userIdClaim);
                
                // Fetch the user entity from the database using IUserRepository
                var userRepository = _serviceProvider.GetRequiredService<IUserRepository>();
                CurrentUser = await userRepository.GetByIdAsync(currentUserId);
            }

            // Resolve ITenantAccessor to get the current tenantEntity (subdomain or tenantEntity identifier)
            var tenantAccessor = _serviceProvider.GetService<ITenantAccessor>();
            if (tenantAccessor != null && !string.IsNullOrEmpty(tenantAccessor.Tenant))
            {
                // Fetch the tenantEntity entity from the repository using ITenantRepository
                var tenantRepository = _serviceProvider.GetRequiredService<ITenantRepository>();
                CurrentTenant = (await tenantRepository.GetTenantBySubdomainAsync(tenantAccessor.Tenant))!;
                
            }
        }

        // Method to validate that both tenantEntity exists and user is involved in this tenantEntity
        protected async Task<TenantMemberEntity> ValidateTenantAccessAsync(string requiredPermission)
        {

             ValidateUser();

             ValidateTenant();
             
            // Check if the user is involved in the tenantEntity and has the required permission
            var tenantMemberRepository = _serviceProvider.GetRequiredService<ITenantMemberRepository>();
            var tenantMember = await tenantMemberRepository.GetUserTenantByUserIdAndTenantIdAsync(CurrentUser.Id, CurrentTenant.Id);

            if (tenantMember == null)
            {
                throw new UnauthorizedAccessException("User is not a member of the current tenantEntity.");
            }

            if (!tenantMember.HasPermission(requiredPermission))
            {
                throw new UnauthorizedAccessException($"User does not have the required permission: {requiredPermission}");
            }

            return tenantMember;
        }

        // Method to validate that the user exists
        protected void ValidateUser()
        {
            if (CurrentUser == null)
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }
        }


        // Method to validate that the tenantEntity exists
        protected void ValidateTenant()
        {
            if (CurrentTenant == null)
            {
                throw new TenantNotFoundException("Tenant is required for this operation.");
            }
        }

        // Property to check if the tenantEntity is available without throwing
        protected bool IsTenantAvailable => CurrentTenant != null;

        // Property to check if the user is available without throwing
        protected bool IsUserAvailable => CurrentUser != null;
    }
}
