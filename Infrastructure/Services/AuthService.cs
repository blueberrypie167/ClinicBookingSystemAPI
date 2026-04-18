using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Infrastructure.Repositories;

namespace Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        public User user = new User();

        private readonly IUserRepository _repository;

        public AuthService(IUserRepository repository)
        {
            _repository = repository;
        }
        
        public async Task SaveChangesAsync()
        {
            await Task.CompletedTask;
        }
        public async Task<User> RegisterUserAsync(userDTO request)
        {
            var hashedpassword = new PasswordHasher<User>().HashPassword(user, request.Password);

            user.Username = request.Username;
            user.PasswordHash = hashedpassword;

            await _repository.CreateUserAsync(user);

            return user;
        }
        public async Task<AuthResultDTO> LoginUserAsync(userDTO request)
        {
            var user = await _repository.GetUserAsync(request.Username);
            if (user == null)
            {
                return new AuthResultDTO { Success = false, Error = "User not found" };
            }
            else if (new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, request.Password)
                == PasswordVerificationResult.Failed)
            {
                return new AuthResultDTO { Success = false, Error = "Invalid Credentials" };
            }
            
            // string token = GenerateJwtToken(user); todo
            return new AuthResultDTO { Success = true};
            
        }
    }
}
