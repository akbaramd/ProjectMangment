using SharedKernel.DomainDrivenDesign.Domain.Repository;

namespace PMS.Domain.BoundedContexts.UserManagment.Repositories
{
    public interface IUserRepository : IGenericRepository<ApplicationUser>
    {
        // Add any specific methods related to User if needed
        Task<ApplicationUser?> GetUserByPhoneNumberAsync(string phoneNumber);
        Task<ApplicationUser?> GetUserByEmailAsync(string email);
    }
}