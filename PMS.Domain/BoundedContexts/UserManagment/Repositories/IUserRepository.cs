using Bonyan.Layer.Domain.Abstractions;

namespace PMS.Domain.BoundedContexts.UserManagment.Repositories
{
    public interface IUserRepository : IRepository<UserEntity,Guid>
    {
        // Add any specific methods related to UserEntity if needed
        Task<UserEntity?> GetUserByPhoneNumberAsync(string phoneNumber);
        Task<UserEntity?> GetUserByEmailAsync(string email);
        Task<IEnumerable<string>> GetUserRolesAsync(Guid userId);
        Task<bool> CheckPasswordAsync(UserEntity userEntity, string password);
    }
}
