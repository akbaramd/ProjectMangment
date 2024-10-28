using Bonyan.DomainDrivenDesign.Domain.Abstractions;

namespace PMS.Domain.BoundedContexts.UserManagment.Repositories
{
    public interface IUserRepository : IRepository<ApplicationUser,Guid>
    {
        // Add any specific methods related to User if needed
        Task<ApplicationUser?> GetUserByPhoneNumberAsync(string phoneNumber);
        Task<ApplicationUser?> GetUserByEmailAsync(string email);
        Task<IEnumerable<string>> GetUserRolesAsync(Guid userId);
        Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
    }
}
