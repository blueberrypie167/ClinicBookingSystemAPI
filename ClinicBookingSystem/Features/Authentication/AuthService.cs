using Common.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Features.Authentication
{
    public class AuthService
    {
        private readonly IUserRepository _repository;
        private readonly IConfiguration _config;

        public AuthService(IUserRepository repository, IConfiguration config)
        {
            _repository = repository;
            _config = config;
        }
        
        public async Task<AuthResultDTO> RegisterUserAsync(userDTO request)
        {
            var user = new User();

            if (await _repository.GetUserAsync(request.Username) is not null){
                return new AuthResultDTO { Success = false, Message = "Username already exists" };
            }
            var hashedPassword = new PasswordHasher<User>().HashPassword(user, request.Password);

            user.Username = request.Username;
            user.PasswordHash = hashedPassword;
            
            await _repository.CreateUserAsync(user);

            var token = GenerateJwtToken(user);
            return new AuthResultDTO { Success = true, Token = token , Message = "User is Created"};
        }
        public async Task<AuthResultDTO> LoginUserAsync(userDTO request)
        {
            var user = await _repository.GetUserAsync(request.Username);
            if (user == null)
            {
                return new AuthResultDTO { Success = false, Message = "User not found" };
            }

            var verificationResult = new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, request.Password);

            if (verificationResult == PasswordVerificationResult.Failed)
            {
                return new AuthResultDTO { Success = false, Message = "Invalid Credentials" };
            }

            // jwt token
            var token = GenerateJwtToken(user);
            return new AuthResultDTO { Success = true, Token = token , Message = "Successful login"};

        }
        private string GenerateJwtToken(User user)
        {
            
            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, user.userId.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.userRole.ToString())
            
            };

            var jwtSettings = _config.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(1200),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
