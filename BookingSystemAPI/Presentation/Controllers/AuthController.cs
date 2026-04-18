using Domain.Entities;
using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace BookingSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(userDTO request)
        {
            User user = await _authService.RegisterUserAsync(request);

            return StatusCode(201, user);
        }

        [HttpPost("login")]
        [Authorize]
        public async Task<IActionResult> Login(userDTO request)
        {
            var result = await _authService.LoginUserAsync(request);

            if(result.Success == false)
            {
                return StatusCode(401, result.Error);
            }

            return StatusCode(200, result);
        }
    }
}
