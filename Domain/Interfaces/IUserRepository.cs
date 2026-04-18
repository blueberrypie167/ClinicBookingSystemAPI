using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interfaces
{
    public interface IUserRepository
    {
        public Task<User> GetUserAsync(string username);
       

        public Task<User> CreateUserAsync(User user);

        public Task<int> SaveChangesAsync();
    }
}
