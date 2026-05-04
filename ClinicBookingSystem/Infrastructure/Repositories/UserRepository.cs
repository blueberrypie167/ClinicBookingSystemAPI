using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly userDbContext _context;

        public UserRepository(userDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.users.ToListAsync();
        }

        public async Task<User> CreateUserAsync(User user)
        {
            await _context.users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User?> GetUserAsync(string username)
        {
            var result = await _context.users.FirstOrDefaultAsync(u => u.Username == username);
            if (result is null)
            {
                return null;
            }
            return result;
        }

        public async Task<User> UpdateUserRole(Guid userId, UserRole role)
        {
            var user = await _context.users.FirstOrDefaultAsync(u => u.userId == userId);

            if(user is null)
            {
                throw new Exception("User not found");
            }

            user.userRole = role;
            

            return user;
        }

    }
}
