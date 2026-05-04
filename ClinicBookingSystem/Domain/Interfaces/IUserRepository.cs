using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IUserRepository
    {
        public Task<User?> GetUserAsync(string username);
       
        public Task<User> CreateUserAsync(User user);

        public Task<IEnumerable<User>> GetAllUsersAsync();

        public Task<User> UpdateUserRole(Guid userId, UserRole role);

    }
}
