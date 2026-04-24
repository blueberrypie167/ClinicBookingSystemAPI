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
            var result = await _authService.RegisterUserAsync(request);
            
            if(result.Success is false)
            {
                return BadRequest(result.Message);
            }

            return Created(result.Message, result);
        }

        [HttpPost("login")]
     
        public async Task<IActionResult> Login(userDTO request)
        {
            var result = await _authService.LoginUserAsync(request);

            if(result.Success == false)
            {
                return BadRequest(result.Message);
            }

            return Ok(result);
        }
        [Authorize]
        [HttpGet]
        public IActionResult Authenticated()
        {
            return Ok("You are Authenticated");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("admin-only")]
        public IActionResult AdminAuthenticated()
        {
            return Ok("You are Admin");
        }
    }
}
