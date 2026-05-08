using ePizza.Domain.Models;

namespace ePizza.Domain.Interfaces
{
    public interface IUserRepository : IGenericRepository<UserDomain>
    {
        Task<UserDomain> GetUserByEmailAsync(string emailAddress);

        Task<int> AddUserAsync(UserDomain user);
    }
}
