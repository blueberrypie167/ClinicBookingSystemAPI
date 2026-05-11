using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ClinicBookingSystem.Features.Authentication;

namespace ClinicBookingSystem.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
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

            if(result.Success is false)
            {
                return BadRequest(result.Message);
            }

            return Ok(result);
        }
        [Authorize]
        [HttpGet("test/authentication-all-roles")]
        public IActionResult Authenticated()
        {
            return Ok("You are Authenticated");
        }
    }
}
