using PMS.Domain.Entities;
using SharedKernel.Domain;
using System.Threading.Tasks;

namespace PMS.Domain.Repositories
{
    public interface IUserRepository : IGenericRepository<ApplicationUser>
    {
        // Add any specific methods related to User if needed
        Task<ApplicationUser?> GetUserByPhoneNumberAsync(string phoneNumber);
        Task<ApplicationUser?> GetUserByEmailAsync(string email);
    }
}
