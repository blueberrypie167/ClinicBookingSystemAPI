using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ClinicBookingSystem.Features.Authentication
{
    public class AuthService
    {
        private readonly IUserRepository _repository;
        private readonly IConfiguration _config;
        private readonly IUnitOfWork _unitOfWork;

        public AuthService(IUnitOfWork unitOfWork, IUserRepository repository, IConfiguration config)
        {
            _repository = repository;
            _config = config;
            _unitOfWork = unitOfWork;
        }
        
        public async Task<AuthResultDTO> RegisterUserAsync(userDTO request)
        {
            if (await _repository.GetUserAsync(request.Username) is not null)
            {
                return new AuthResultDTO { Success = false, Message = "Username already exists" };
            }

            // create user with passwordhash
            var hashedPassword = new PasswordHasher<User>().HashPassword(null, request.Password);

            await using var tx = await _unitOfWork.BeginTransactionAsync();
            try
            {
                var user = new User(request.Username, hashedPassword, UserRole.Patient);

                await _repository.CreateUserAsync(user);

                await _unitOfWork.SaveChangesAsync();

                await tx.CommitAsync();

                var token = GenerateJwtToken(user);

                return new AuthResultDTO 
                { 
                    Success = true, 
                    Token = token,
                    Role = user.userRole.ToString(),
                    Message = "User is Created" 
                };
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }
        public async Task<AuthResultDTO> LoginUserAsync(userDTO request)
        {
            // get user object by username
            var user = await _repository.GetUserAsync(request.Username);

            if (user == null)
            {
                return new AuthResultDTO { Success = false, Message = "User not found" };
            }
            
            // create hasher
            var hasher = new PasswordHasher<User>();

            // verify request password with the hashed password of the user specified
            if (!user.VerifyPassword(request.Password, hasher))
            {
                return new AuthResultDTO { Success = false, Message = "Invalid Credentials" };
            }

            // jwt token
            var token = GenerateJwtToken(user);
            return new AuthResultDTO 
            { 
                Success = true, 
                Token = token,
                Role = user.userRole.ToString(),
                Message = "Successful login"
            };

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
